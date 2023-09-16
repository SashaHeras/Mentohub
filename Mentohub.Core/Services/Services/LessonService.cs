using Mentohub.Core.Repositories.Repositories;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Mentohub.Core.Services.Services
{
    public class LessonService
    {
        private readonly LessonRepository _lessonRepository;

        private readonly CourseService _courseService;
        private readonly AzureService _azureService;
        private readonly MediaService _mediaService;
        private readonly CourseItemService _courseItemService;

        public LessonService(
            LessonRepository lessonRepository,
            CourseService courseService,
            AzureService azureService,
            MediaService mediaService,
            CourseItemService courseItemService
            )
        {
            _lessonRepository = lessonRepository;
            _courseService = courseService;
            _azureService = azureService;
            _mediaService = mediaService;
            _courseItemService = courseItemService;
        }     

        public async Task<int> Create(IFormCollection form, LessonDTO createLessonModel)
        {
            string videoName = await _azureService.SaveInAsync(createLessonModel.VideoFile);
            CourseItem newCourceItem = await _courseItemService.Create(createLessonModel);

            Lesson newLesson = new Lesson()
            {
                Id = Guid.NewGuid(),
                Theme = createLessonModel.Theme,
                Description = createLessonModel.Description,
                Body = createLessonModel.Body,
                VideoPath = videoName,
                CourseItemId = newCourceItem.Id,
                DateCreation = DateTime.Now.ToShortDateString()
            };

            await _lessonRepository.AddAsync(newLesson);

            return newCourceItem.CourseId;
        }

        public async Task<int> Edit(IFormCollection form, Lesson lesson)
        {
            if (form.Files.Count != 0)
            {
                await _azureService.DeleteFromAzure(lesson.VideoPath);
                lesson.VideoPath = await _azureService.SaveInAsync(form.Files[0]);
            }

            CourseItem currentCourseItem = _courseItemService.GetCourseItem(lesson.CourseItemId);
            currentCourseItem.DateCreation = DateTime.Now;
            await _courseItemService.UpdateCourseItem(currentCourseItem);

            int courseId = currentCourseItem.CourseId;

            UpdateLesson(lesson);

            _mediaService.DeleteMediaFromProject(form.Files[0]);

            return courseId;
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

        public async void UpdateLesson(Lesson newLesson)
        {
            Lesson currentLesson = _lessonRepository.GetLessonById(newLesson.Id);

            currentLesson.Theme = newLesson.Theme;
            currentLesson.Description = newLesson.Description;
            currentLesson.Body = newLesson.Body;
            currentLesson.CourseItemId = newLesson.CourseItemId;
            currentLesson.DateCreation = DateTime.Now.ToShortDateString();

            await _lessonRepository.UpdateAsync(currentLesson);
        }       

        public LessonDTO GetLessonByCourseItem(int courseItemId)
        {
            var data = _lessonRepository.GetLessonByCourseItemId(courseItemId);

            return new LessonDTO()
            {
                Id = data.Id,
                Theme = data.Theme,
                Description = data.Description,
                Body = data.Body,
                VideoPath = data.VideoPath,
                CourseItemId = data.CourseItemId,
                DateCreation = data.DateCreation
            };
        }
    }
}
