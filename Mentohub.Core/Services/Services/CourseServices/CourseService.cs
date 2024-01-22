using System.ComponentModel;
using System.Text.Json;
using System.Threading;
using Azure.Core;
using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Core.Repositories.Interfaces;
using Mentohub.Core.Repositories.Interfaces.CourseInterfaces;
using Mentohub.Core.Repositories.Repositories;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Core.Services.Interfaces.CourseInterfaces;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.DTO.CourseDTOs;
using Mentohub.Domain.Data.Entities;
using Mentohub.Domain.Data.Entities.CourseEntities;
using Mentohub.Domain.Data.Enums;
using Mentohub.Domain.Filters;
using Mentohub.Domain.Helpers;
using Mentohub.Domain.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Net.Mime.MediaTypeNames;

namespace Mentohub.Core.Services.Services.CourseServices
{
    public class CourseService : ICourseService
    {
#pragma warning disable 8601
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseItemRepository _courseItemRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly ITestRepository _testRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ICourseLanguageRepository _courseLanguageRepository;
        private readonly ICourseLevelRepository _courseLevelRepository;
        private readonly ICRUD_UserRepository _cRUD_UserRepository;

        private readonly IMediaService _mediaService;
        private readonly ICourseViewService _courseViewsService;
        private readonly ICourseSubjectService _courseSubjectService;
        private readonly ICourseLanguageService _courseLanguageService;
        private readonly ICourseLevelService _courseLevelService;

        public CourseService(
            ICourseRepository courseRepository,
            ICourseItemRepository courseItemRepository,
            ICommentRepository commentRepository,
            ILessonRepository lessonRepository,
            ITestRepository testRepository,
            ISubjectRepository subjectRepository,
            ICourseViewService courseViewsService,
            ICourseLanguageRepository courseLanguageRepository,
            IMediaService mediaService,
            ICourseLevelRepository courseLevelRepository,
            ICourseSubjectService courseSubjectService,
            ICourseLanguageService courseLanguageService,
            ICourseLevelService courseLevelService,
            ICRUD_UserRepository cRUD_UserRepository)

        {
            _courseRepository = courseRepository;
            _courseItemRepository = courseItemRepository;
            _lessonRepository = lessonRepository;
            _commentRepository = commentRepository;
            _testRepository = testRepository;
            _courseViewsService = courseViewsService;
            _mediaService = mediaService;
            _subjectRepository = subjectRepository;
            _courseLanguageRepository = courseLanguageRepository;
            _courseLevelRepository = courseLevelRepository;
            _cRUD_UserRepository = cRUD_UserRepository;

            _courseSubjectService = courseSubjectService;
            _courseLanguageService = courseLanguageService;
            _courseLevelService = courseLevelService;
        }

        public async Task<CourseDTO> Apply(CourseDTO courseDTO)
        {
            var lang = _courseLanguageRepository.FindById(courseDTO.LanguageId)
                                                 ?? throw new Exception("Unknown language!");

            var level = _courseLevelRepository.FindById(courseDTO.CourseLevelId)
                                                ?? throw new Exception("Unknown level!");

            var currentUserID = MentoShyfr.Decrypt(courseDTO.AuthorId);

            var currentCourse = _courseRepository.FirstOrDefault(x => x.Id == courseDTO.Id);
            if (currentCourse == null)
            {
                currentCourse = new Course()
                {
                    Name = courseDTO.Name,
                    AuthorId = currentUserID,
                    Checked = false,
                    Rating = 0.00,
                    Price = courseDTO.Price,
                    ShortDescription = courseDTO.ShortDescription,
                    Description = courseDTO.Description,
                    CourseSubjectId = courseDTO.CourseSubjectId,
                    LastEdittingDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                    LanguageID = courseDTO.LanguageId,
                    CourseLevelID = courseDTO.CourseLevelId
                };

                await SaveFiles(currentCourse, courseDTO);

                currentCourse.LoadPictureName = courseDTO.Picture.FileName;
                currentCourse.LoadVideoName = courseDTO.PreviewVideo.FileName;

                _courseRepository.Add(currentCourse);
            }
            else
            {
                currentCourse.Name = courseDTO.Name;
                currentCourse.Checked = courseDTO.Checked;
                currentCourse.Rating = courseDTO.Rating;
                currentCourse.Price = courseDTO.Price;
                currentCourse.Description = courseDTO.Description;
                currentCourse.ShortDescription = courseDTO.ShortDescription;
                currentCourse.LastEdittingDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                currentCourse.CourseSubjectId = courseDTO.CourseSubjectId;
                currentCourse.LanguageID = courseDTO.LanguageId;
                currentCourse.CourseLevelID = courseDTO.CourseLevelId;

                await SaveOrDeleteCourseMedia(currentCourse, courseDTO);

                _courseRepository.Update(currentCourse);
            }

            courseDTO.Id = currentCourse.Id;
            courseDTO.LoadVideoName = currentCourse.LoadVideoName;
            courseDTO.LoadPictureName = currentCourse.LoadPictureName;
            courseDTO.LastEdittingDate = currentCourse.LastEdittingDate;

            return courseDTO;
        }

        private async Task<bool> SaveOrDeleteCourseMedia(Course currentCourse, CourseDTO courseDTO)
        {
            bool result = true;
            try
            {
                if (currentCourse.LoadPictureName == null || courseDTO.Picture.FileName != currentCourse.LoadPictureName)
                {
                    if (currentCourse.LoadPictureName == null)
                    {
                        await _mediaService.DeleteFile(currentCourse.PicturePath);
                    }

                    currentCourse.PicturePath = await _mediaService.SaveFile(courseDTO.Picture);
                    currentCourse.LoadPictureName = courseDTO.Picture.FileName;
                }

                if (currentCourse.LoadVideoName == null || courseDTO.PreviewVideo.FileName != currentCourse.LoadVideoName)
                {
                    if (currentCourse.LoadVideoName == null)
                    {
                        await _mediaService.DeleteFile(currentCourse.PreviewVideoPath);
                    }

                    currentCourse.PreviewVideoPath = await _mediaService.SaveFile(courseDTO.PreviewVideo);
                    currentCourse.LoadVideoName = courseDTO.PreviewVideo.FileName;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        private async Task<bool> SaveFiles(Course currentCourse, CourseDTO courseDTO)
        {
            currentCourse.PicturePath = await _mediaService.SaveFile(courseDTO.Picture);
            currentCourse.PreviewVideoPath = await _mediaService.SaveFile(courseDTO.PreviewVideo);

            return true;
        }

        /// <summary>
        /// Execute stored procedure to generate list of course elements
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>
        /// Elements list in Json format
        /// </returns>
        public List<CourseElementDTO> GetCourseElements(int Id)
        {
            var result = new List<CourseElementDTO>();
            var course = _courseRepository.FirstOrDefault(x => x.Id == Id)
                                           ?? throw new Exception("Course not found");

            var courseItemsIDs = course.CourseItems.Select(x => x.Id).ToList();
            var tests = _testRepository.GetAll(x => courseItemsIDs.Contains(x.CourseItemId)).ToList();
            var lessons = _lessonRepository.GetAll(x => courseItemsIDs.Contains(x.CourseItemId)).ToList();

            if (tests.Count != 0 || lessons.Count != 0)
            {
                foreach (var t in tests)
                {
                    result.Add(new CourseElementDTO()
                    {
                        CourseItemId = t.CourseItemId,
                        TypeId = (int)e_ItemType.Test,
                        CourseId = course.Id,
                        DateCreation = t.CourseItem.DateCreation,
                        OrderNumber = t.CourseItem.OrderNumber,
                        ElementName = t.Name
                    });
                }

                foreach (var l in lessons)
                {
                    result.Add(new CourseElementDTO()
                    {
                        CourseItemId = l.CourseItemId,
                        TypeId = (int)e_ItemType.Lesson,
                        CourseId = course.Id,
                        DateCreation = l.CourseItem.DateCreation,
                        OrderNumber = l.CourseItem.OrderNumber,
                        ElementName = l.Theme
                    });
                }
            }

            result = result.OrderBy(x => x.OrderNumber).ToList();

            return result;
        }

        public List<CommentDTO> GetCourseComments(int courseID, int count = 10)
        {
            var comments = _commentRepository.GetAll().Where(x => x.CourseId == courseID).ToList();
            var result = new List<CommentDTO>();
            foreach (var com in comments)
            {
                result.Add(new CommentDTO()
                {
                    Text = com.Text,
                    Rating = com.Rating,
                    AuthorId = com.UserId.ToString(),
                    Id = com.Id,
                    CourseId = com.CourseId,
                    UserName = "User",
                    DateAgo = Helper.GetTimeSinceDate(com.DateCreation),
                    ProfileImagePath = "img_avatar.png"
                });
            }

            result = result.Take(count).ToList();

            return result;
        }

        public IQueryable<Course> GetAuthorsCourses(Guid userId)
        {
            throw new NotImplementedException();
        }

        public List<CourseDTO> GetUserCourses(string userId)
        {
            var result = new List<CourseDTO>();
            var courses = _courseRepository.GetAllAuthorsCourses(userId).ToList();
            foreach (var course in courses)
            {
                result.Add(new CourseDTO()
                {
                    Id = course.Id,
                    Name = course.Name,
                    Price = course.Price,
                    Rating = course.Rating,
                    PicturePath = course.PicturePath,
                    AuthorName = "User",
                });
            }

            return result;
        }

        public async Task<CourseDTO> ViewCourse(int CourseID, string UserID)
        {
            var course = _courseRepository.FirstOrDefault(x => x.Id == CourseID)
                                           ?? throw new Exception("Course not found!");

            var result = CourseMapper.ToDTO(course);

            var courseView = await _courseViewsService.TryAddUserView(CourseID, UserID);
            if (courseView != null)
            {
                course.CourseViews = course.CourseViews == null ? new List<CourseViews>() : course.CourseViews;

                course.CourseViews.Add(courseView);
                _courseRepository.Update(course);
            }

            result.CourseViews = course.CourseViews.Count;
            GetAdditionalLists(result);

            return result;
        }

        private void GetAdditionalLists(CourseDTO course)
        {
            course.SubjectsList = _subjectRepository.GetAll()
               .Select(x => new CourseSubjectDTO()
               {
                   Id = x.Id,
                   Name = x.Name
               }).ToList();

            course.CourseElementsList = GetCourseElements(course.Id);
        }

        public List<CourseDTO> MostFamoustList()
        {
            const int countToTake = 15;
            var courses = _courseRepository.GetAll()
                                           .OrderBy(x => x.CourseViews.Count)
                                           .Take(countToTake)
                                           .Select(x => new CourseDTO()
                                           {
                                               Id = x.Id,
                                               Name = x.Name,
                                               CourseViews = x.CourseViews.Count,
                                               Rating = x.Rating,
                                               AuthorId = x.AuthorId.ToString(),
                                               Price = x.Price,
                                               PicturePath = x.PicturePath,
                                           })
                                           .ToList();

            return courses;
        }

        public List<CourseBlockDTO> GetCourseInfoList(int ID)
        {
            var course = _courseRepository.FirstOrDefault(x => x.Id == ID)
                                           ?? throw new Exception("Unknown course!");

            var itemsIdList = course.CourseItems.Select(x => x.Id).ToList();
            var courseItems = _courseItemRepository.GetAll(x => itemsIdList.Contains(x.Id))
                                                   .Include(x => x.Test)
                                                   .Include(x => x.Lesson)
                                                   .ToList();

            var blocks = course.CourseBlocks.Select(x => CourseMapper.ToDTO(x)).ToList();
            foreach (var bl in blocks)
            {
                var currentBlockItems = courseItems.Where(x => x.CourseBlockID == bl.ID).ToList();
                foreach (var item in currentBlockItems)
                {
                    if (item.Test != null)
                    {
                        bl.CourseItems.Add(new CourseElementDTO()
                        {
                            CourseItemId = item.Test.CourseItemId,
                            TypeId = (int)e_ItemType.Test,
                            CourseId = item.CourseId,
                            DateCreation = item.DateCreation,
                            OrderNumber = item.OrderNumber,
                            ElementName = item.Test.Name
                        });

                        bl.TestCount++;
                    }
                    else if (item.Lesson != null)
                    {
                        bl.CourseItems.Add(new CourseElementDTO()
                        {
                            CourseItemId = item.Lesson.CourseItemId,
                            TypeId = (int)e_ItemType.Lesson,
                            CourseId = item.CourseId,
                            DateCreation = item.DateCreation,
                            OrderNumber = item.OrderNumber,
                            ElementName = item.Lesson.Theme
                        });

                        bl.LessonsCount++;
                    }
                }
            }

            var result = blocks;

            return result;
        }

        public List<CourseDTO> List(SearchFilterModel filter, out int totalCount)
        {
            var coursesList = _courseRepository.GetAll(
                    x => x.Checked == true &&
                    x.Price >= filter.PriceFrom &&
                    x.Price <= filter.PriceTo &&
                    (filter.SearchText == string.Empty ? true :
                        
                            x.Name.Contains(filter.SearchText) ||
                            x.CourseTags.Select(x => x.Tag.Name).Contains(filter.SearchText) ||
                            x.Category.Name.Contains(filter.SearchText)
                        
                    ) &&
                    (filter.CategoryID == -1 ? true : x.CourseSubjectId == filter.CategoryID) &&
                    (filter.Level == -1 ? true : x.CourseLevelID == filter.Level) &&
                    (filter.LanguageID == -1 ? true : x.LanguageID == filter.LanguageID) &&
                    (filter.Rate == -1 ? true : x.Rating > filter.Rate)
                );

            if (filter.SortOption != 0)
            {
                if (filter.SortOption == (int)e_SortOptions.Rate)
                {
                    coursesList = coursesList.OrderBy(x => x.Rating);
                }
                else if (filter.SortOption == (int)e_SortOptions.Views)
                {
                    coursesList = coursesList.OrderBy(x => x.CourseViews.Count);
                }
                else if (filter.SortOption == (int)e_SortOptions.DateUp)
                {
                    coursesList = coursesList.OrderByDescending(x => x.LastEdittingDate);
                }
                else if (filter.SortOption == (int)e_SortOptions.DateDown)
                {
                    coursesList = coursesList.OrderBy(x => x.LastEdittingDate);
                }
            }

            totalCount = coursesList.Count();
            var result = coursesList.Skip(filter.Count * filter.CurrentPage)
                                    .Take(filter.Count)
                                    .ToList()
                                    .Select(x => CourseMapper.ToSimpleCourseDTO(x))
                                    .ToList();

            return result;
        }

        public SearchCourseFilterData InitSearchFilterData()
        {
            var filter = new SearchCourseFilterData();
            filter.Categories = _courseSubjectService.SubjectsList();
            filter.Categories.Insert(0, new KeyValuePair<int, string>(-1, "Будь-який"));

            filter.Languages = _courseLanguageService.GetLanguagesList();
            filter.Languages.Insert(0, new KeyValuePair<int, string>(-1, "Будь-яка"));

            filter.Levels = _courseLevelService.GetLevelsList();
            filter.Levels.Insert(0, new KeyValuePair<int, string>(-1, "Будь-який"));

            return filter;
        }

        public List<CourseDTO> GetAuthorsToCourses(string authorID)
        {
            var userID = MentoShyfr.Decrypt(authorID);
            var courses = _courseRepository.GetAll(x => x.AuthorId == userID)
                                           .ToList();

            var result = courses.Select(x => CourseMapper.ToDTO(x))
                                .OrderBy(x => x.Rating)
                                .Take(6)
                                .ToList();

            return result;
        }

        public AuthorInfoDTO GetAuthorInfoDTO(string encriptId)
        {
            var authorId = MentoShyfr.Decrypt(encriptId);
            if (authorId == null)
            {
                throw new Exception("Id not found");
            }
            CurrentUser author = _cRUD_UserRepository.FindByID(authorId);
            var authorInfoDTO = new AuthorInfoDTO();
            authorInfoDTO.Id = encriptId;
            authorInfoDTO.Image = author.Image;
            authorInfoDTO.FirstName = author.FirstName;
            authorInfoDTO.LastName = author.LastName;
            authorInfoDTO.AboutMe = author.AboutMe;
            authorInfoDTO.CountOfStudents = 0;
            var courses = _courseRepository.GetAllAuthorsCourses(authorId).ToList();
            double averageRating;
            if (courses == null)
            {
                averageRating = 0.0;
            }
            else
            {
                averageRating = courses.Average(c => c.Rating);
            }
            authorInfoDTO.AvarageRating = averageRating;

            return authorInfoDTO;
        }
    }
}
