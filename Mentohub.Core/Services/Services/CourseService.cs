using System.Text.Json;
using Mentohub.Core.Repositories.Repositories;
using Mentohub.Domain.Entities;

namespace Mentohub.Core.Services.Services
{
    public class CourseService
    {
        public CourseRepository _courseRepository;
        public CourseItemRepository _courseItemRepository;
        public CourseTypeRepository _courseTypeRepository;
        public LessonRepository _lessonRepository;

        private CourseItemService _courseItemService;

        public CourseService(CourseRepository courseRepository, 
            CourseItemRepository courseItemRepository, 
            CourseTypeRepository courseTypeRepository, 
            LessonRepository lessonRepository,
            CourseItemService courseItemService)
        {
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

        public Course GetCourse(int id)
        {
            return _courseRepository.GetCourse(id);
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
        public List<CourseElement>? GetCourseElements(int id)
        {
            var elementsJson = _courseRepository.GetCourseElementsList(id.ToString());
            var result = JsonSerializer.Deserialize<List<CourseElement>>(elementsJson);
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
        public Course CreateNewCourse()
        {
            return new Course()
            {
                Id = 0,
                Name = "",
                Price = 0
            };
        }
    }
}
