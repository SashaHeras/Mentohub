using System.Text.Json;
using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Repositories;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mentohub.Core.Services.Services
{
    public class CourseService
    {
        private readonly ProjectContext _context;

        public CourseRepository _courseRepository;
        public CourseItemRepository _courseItemRepository;
        public CourseTypeRepository _courseTypeRepository;
        public LessonRepository _lessonRepository;

        private CourseItemService _courseItemService;

        public CourseService(
            ProjectContext context,
            CourseRepository courseRepository, 
            CourseItemRepository courseItemRepository, 
            CourseTypeRepository courseTypeRepository, 
            LessonRepository lessonRepository,
            CourseItemService courseItemService)
        {
            this._context = context;
            _courseRepository = courseRepository;
            _courseItemRepository = courseItemRepository;
            _courseTypeRepository = courseTypeRepository;
            _lessonRepository = lessonRepository;
            _courseItemService = courseItemService;
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
            if(entityList != null)
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
    }
}
