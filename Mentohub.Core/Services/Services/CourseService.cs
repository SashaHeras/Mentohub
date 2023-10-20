using System.ComponentModel;
using System.Text.Json;
using System.Threading;
using Azure.Core;
using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Repositories;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Net.Mime.MediaTypeNames;

namespace Mentohub.Core.Services.Services
{
    public class CourseService
    {
        private readonly ProjectContext _context;

        private readonly CourseRepository _courseRepository;
        private readonly CourseItemRepository _courseItemRepository;
        private readonly CourseTypeRepository _courseTypeRepository;
        private readonly LessonRepository _lessonRepository;
        //private readonly MediaService _mediaService;

        private readonly CourseItemService _courseItemService;

        public CourseService(
            ProjectContext context,
            CourseRepository courseRepository,
            CourseItemRepository courseItemRepository,
            CourseTypeRepository courseTypeRepository,
            LessonRepository lessonRepository,
            CourseItemService courseItemService/*,*/
            /*MediaService mediaService*/)
        {
            this._context = context;
            _courseRepository = courseRepository;
            _courseItemRepository = courseItemRepository;
            _courseTypeRepository = courseTypeRepository;
            _lessonRepository = lessonRepository;
            _courseItemService = courseItemService;
            //_mediaService = mediaService;
        }

        public IQueryable<Course> GetAuthorsCourses(Guid userId)
        {
            return _courseRepository.GetAllAuthorsCourses(userId);
        }

        public async Task<Course> AddCourse(Course course)
        {
            return await _courseRepository.AddAsync(course);
        }

        public async Task<Course> UpdateCourse(Course course)
        {
            return await _courseRepository.UpdateAsync(course);
        }

        public CourseDTO GetCourse(int id)
        {
            CourseDTO res = new CourseDTO();

            if (id == 0)
            {
                res = CreateNewCourse();
            }
            else
            {
                Course currCourse = _courseRepository.GetCourse(id);

                res = currCourse.ToDTO();
                res.CourseElementsList = GetCourseElements(id);
                res.DefaultTypeId = res.CourseElementsList[0].TypeId;
                res.DefaultCourseItemId = res.CourseElementsList[0].CourseItemId;
            }

            res.SubjectsList = GetSubjectsList();

            return res;
        }

        public Course GetCourseFromLesson(Lesson lesson)
        {
            var courseId = _courseItemService.GetCourseItem(lesson.CourseItemId).CourseId;
            return _courseRepository.GetCourse(courseId);
        }

        /// <summary>
        /// Execute stored procedure to generate list of course elements
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// Elements list in Json format
        /// </returns>
        public List<CourseElementDTO> GetCourseElements(int id)
        {
            var elementsJson = _courseRepository.GetCourseElementsList(id.ToString());
            var entityList = JsonSerializer.Deserialize<List<CourseElement>>(elementsJson);
            var result = new List<CourseElementDTO>();
            if (entityList != null)
            {
                foreach (var e in entityList)
                {
                    result.Add(new CourseElementDTO()
                    {
                        CourseItemId = e.CourseItemId,
                        TypeId = e.TypeId,
                        CourseId = e.CourseId,
                        DateCreation = e.DateCreation,
                        OrderNumber = e.OrderNumber,
                        ElementName = e.ElementName
                    });
                }
            }

            return result;
        }

        public List<CommentDTO> GetCourseComments(int courseID, int count = 10)
        {
            List<CommentDTO> commentsList = new List<CommentDTO>();
            commentsList = _context.Comments.Where(c => c.CourseId == courseID).
                Select(x=> new CommentDTO()
                {
                    Id = x.Id,
                    Text = x.Text,
                    Rating = x.Rating,
                    DateAgo = GetTimeSinceDate(x.DateCreation),
                    UserName = "",             
                    ProfileImagePath = ""       
                }).ToList();

            if (commentsList.Count == 0)
            {
                commentsList.Add(new CommentDTO()
                {
                    Id = 5,
                    Text = "Hello",
                    Rating = 4,
                    DateAgo = "4 month ago",
                    UserName = "Heras A.",              // Add user name Heras O.
                    ProfileImagePath = ""     // Add user profile image name
                });
            }

            return commentsList;
        }

        public async Task<int> SaveCource(IFormCollection form)
        {
            int courseId = Convert.ToInt32(form["courseId"]);
            FilesHandler handler = new FilesHandler(form.Files, courseId);
            //handler.SaveFiles(_mediaService);

            Course newCourse = new Course()
            {
                Name = form["name"].ToString(),
                AuthorId = Guid.Parse(form["authorId"].ToString()),
                Checked = false,
                Price = Convert.ToDecimal(form["price"].ToString()),
                CourseSubjectId = Convert.ToInt32(form["subject"]),
                LastEdittingDate = DateTime.Now,
                PicturePath = handler.PictureName,
                PreviewVideoPath = handler.VideoName
            };            

            if (courseId == 0)
            {
                await AddCourse(newCourse);
            }
            else
            {
                newCourse.Id = courseId;
                await UpdateCourse(newCourse);
            }

            return newCourse.Id;
        }

        /// <summary>
        /// Create new empty course
        /// </summary>
        /// <returns>
        /// new Course()
        /// {
        ///    Id = 0,
        ///   Name = "",
        ///   Price = 0
        /// };
        /// </returns>
        /// 
        private List<CourseSubjectDTO> GetSubjectsList()
        {
            List<CourseSubjectDTO> res = new List<CourseSubjectDTO>();
            var subjects = _context.CourseSubjects.ToList();
            foreach (var subject in subjects)
            {
                res.Add(subject.ToDTO());
            }

            return res;
        }

        public CourseDTO CreateNewCourse()
        {
            CourseDTO res = new CourseDTO();            
            res.Id = 0;
            res.Name = "";
            res.Price = 0;

            return res;
        }

        public class FilesHandler
        {
            public string VideoName { get; set; }

            public string PictureName { get; set; }

            public IFormFile Video { get; set; }

            public IFormFile Picture { get; set; }

            public int CourseID { get; set; }

            public FilesHandler(IFormFileCollection formFiles, int courseID)
            {
                if(formFiles.Count > 0)
                {
                    Video = formFiles["video"];
                    Picture = formFiles["picture"];
                }
                CourseID = courseID;
            }

            public async void SaveFiles(MediaService service)
            {
                PictureName = await service.SaveMedia(Picture, CourseID);
                VideoName = await service.SaveMedia(Video, CourseID);
            }
        }

        private static string GetTimeSinceDate(DateTime date)
        {
            TimeSpan timeSpan = DateTime.Now - date;

            int totalDays = (int)timeSpan.TotalDays;
            int totalWeeks = totalDays / 7;
            int totalMonths = totalDays / 30;
            int totalYears = totalDays / 365;

            if (totalWeeks < 4)
            {
                return $"{totalWeeks} week{(totalWeeks == 1 ? "" : "s")} ago";
            }
            else if (totalMonths < 12)
            {
                return $"{totalMonths} month{(totalMonths == 1 ? "" : "s")} ago";
            }
            else
            {
                return $"{totalYears} year{(totalYears == 1 ? "" : "s")} ago";
            }
        }
    }
}
