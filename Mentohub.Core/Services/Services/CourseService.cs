using System.ComponentModel;
using System.Text.Json;
using System.Threading;
using Azure.Core;
using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Core.Repositories.Interfaces;
using Mentohub.Core.Repositories.Repositories;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.DTO.Helpers;
using Mentohub.Domain.Data.DTO.Mappers;
using Mentohub.Domain.Data.Entities;
using Mentohub.Domain.Data.Enums;
using Mentohub.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Net.Mime.MediaTypeNames;

namespace Mentohub.Core.Services.Services
{
    public class CourseService : ICourseService
    {
        private readonly ProjectContext _context;

        private readonly ICourseRepository _courseRepository;
        private readonly ICourseItemRepository _courseItemRepository;
        private readonly ICourseTypeRepository _courseTypeRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly ICourseBlockRepository _courseBlockRepository;
        private readonly ITestRepository _testRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly ICommentRepository _commentRepository;

        private readonly IMediaService _mediaService;
        private readonly ICourseItemService _courseItemService;
        private readonly ICourseViewService _courseViewsService;

        public CourseService(
            ProjectContext context,
            ICourseRepository courseRepository,
            ICourseItemRepository courseItemRepository,
            ICourseTypeRepository courseTypeRepository,
            ICommentRepository commentRepository,
            ILessonRepository lessonRepository,
            ITestRepository testRepository,
            ISubjectRepository subjectRepository,
            ICourseItemService courseItemService,
            ICourseViewService courseViewsService,
            ICourseBlockRepository courseBlockRepository,
            IMediaService mediaService)
        {
            this._context = context;
            _courseRepository = courseRepository;
            _courseItemRepository = courseItemRepository;
            _courseTypeRepository = courseTypeRepository;
            _lessonRepository = lessonRepository;
            _courseItemService = courseItemService;
            _commentRepository = commentRepository;
            _testRepository = testRepository;
            _courseViewsService = courseViewsService;
            _mediaService = mediaService;
            _subjectRepository = subjectRepository;
            _courseBlockRepository = courseBlockRepository;
        }

        public async Task<CourseDTO> Edit(CourseDTO courseDTO)
        {
            var currentCourse = _courseRepository.FirstOrDefault(x => x.Id == courseDTO.Id);
            if(currentCourse == null)
            {
                currentCourse = new Course()
                {
                    Name = courseDTO.Name,
                    AuthorId = courseDTO.AuthorId,
                    Checked = false,
                    Rating = 0.00,
                    Price = courseDTO.Price,
                    CourseSubjectId = (int)courseDTO.CourseSubjectId,
                    LastEdittingDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)
                };

                try
                {
                    currentCourse.PicturePath = await _mediaService.SaveFile(courseDTO.Picture);
                }
                catch(Exception ex)
                {
                    throw ex;
                }

                try
                {
                    currentCourse.PreviewVideoPath = await _mediaService.SaveFile(courseDTO.PreviewVideo);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

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
                currentCourse.LastEdittingDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                currentCourse.CourseSubjectId = courseDTO.CourseSubjectId;
                            
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

                _courseRepository.Update(currentCourse);
            }

            courseDTO.Id = currentCourse.Id;
            courseDTO.LoadVideoName = currentCourse.LoadVideoName;
            courseDTO.LoadPictureName = currentCourse.LoadPictureName;
            courseDTO.LastEdittingDate = currentCourse.LastEdittingDate;

            return courseDTO;
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
            var course = _courseRepository.FirstOrDefault(x => x.Id == Id);
            if(course == null)
            {
                throw new Exception("Course not found");
            }

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

        public List<CourseDTO> GetUserCourses(string userId) {
            var result = new List<CourseDTO>();
            var courses = _courseRepository.GetAllAuthorsCourses(userId).ToList();
            foreach(var course in courses)
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
            var course = _courseRepository.FirstOrDefault(x => x.Id == CourseID);
            if(course == null)
            {
                throw new Exception("Course not found!");
            }

            var result = CourseMapper.ToDTO(course);

            var courseView = await _courseViewsService.TryAddUserView(CourseID, UserID);
            if(courseView != null)
            {
                if(course.CourseViews == null)
                {
                    course.CourseViews = new List<CourseViews>();
                }

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
            var course = _courseRepository.FirstOrDefault(x => x.Id == ID);
            if(course == null)
            {
                throw new Exception("Unknown course!");
            }

            var itemsIdList = course.CourseItems.Select(x => x.Id).ToList();
            var courseItems = _courseItemRepository.GetAll(x => itemsIdList.Contains(x.Id))
                                                   .Include(x=>x.Test)
                                                   .Include(x => x.Lesson)
                                                   .ToList();

            var blocks = course.CourseBlocks.Select(x => CourseMapper.ToDTO(x)).ToList();
            foreach(var bl in blocks)
            {
                var currentBlockItems = courseItems.Where(x => x.CourseBlockID == bl.ID).ToList();
                foreach(var item in currentBlockItems)
                {
                    if(item.Test != null)
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
                    else if(item.Lesson != null)
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
    }
}
