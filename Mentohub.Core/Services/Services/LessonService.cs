using Mentohub.Core.Repositories.Repositories;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Entities;

namespace Mentohub.Core.Services.Services
{
    public class LessonService
    {
        private readonly LessonRepository _lessonRepository;
        private readonly CourseService _courseService;

        public LessonService(
            LessonRepository lessonRepository,
            CourseService courseService)
        {
            _lessonRepository = lessonRepository;
            _courseService = courseService;
        }     

        public void CreateLesson(CourseItem courseItem, LessonDTO lesson, string fileName)
        {
            Lesson newLesson = new Lesson()
            {
                Id = Guid.NewGuid(),
                Theme = lesson.Theme,
                Description = lesson.Description,
                Body = lesson.Body,
                VideoPath = fileName,
                CourseItemId = courseItem.Id,
                DateCreation = DateTime.Now.ToShortDateString()
            };

            _lessonRepository.AddAsync(newLesson);
        }

        public LessonDTO GetLesson(Guid id)
        {           
            var lesson = _lessonRepository.GetLessonById(id);            
            var course = _courseService.GetCourseFromLesson(lesson);

            LessonDTO result = new LessonDTO() {
                Id = lesson.Id,
                Theme = lesson.Theme,
                Description = lesson.Description,
                Body = lesson.Body,
                VideoPath = lesson.VideoPath,
                CourseItemId = lesson.CourseItemId,
                DateCreation = lesson.DateCreation,
                CourseName = course.Name,
                CourseRating = course.Rating,
                CourseID = course.Id
            };

            return result;
        }

        public void UpdateLesson(Lesson newLesson)
        {
            Lesson currentLesson = _lessonRepository.GetLessonById(newLesson.Id);

            currentLesson.Theme = newLesson.Theme;
            currentLesson.Description = newLesson.Description;
            currentLesson.Body = newLesson.Body;
            currentLesson.CourseItemId = newLesson.CourseItemId;
            currentLesson.DateCreation = DateTime.Now.ToShortDateString();

            _lessonRepository.UpdateAsync(currentLesson);
        }       

        public Lesson GetLessonByCourseItem(int courseItemId)
        {
            return _lessonRepository.GetLessonByCourseItemId(courseItemId);
        }
    }
}
