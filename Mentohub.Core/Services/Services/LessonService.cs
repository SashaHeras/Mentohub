using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Core.Repositories.Repositories;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.Data;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Mentohub.Core.Services.Services
{
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly ICourseService _courseService;
        private readonly IMediaService _mediaService;
        private readonly IAzureService _azureService;
        private readonly ICourseItemService _courseItemService;

        private readonly ICourseRepository _courseRepository;
        private readonly ICourseItemRepository _courseItemRepository;

        public LessonService(
            ILessonRepository lessonRepository,
            ICourseService courseService,
            IMediaService mediaService,
            ICourseRepository courseRepository,
            ICourseItemRepository courseItemRepository,
            IAzureService azureService,
            ICourseItemService courseItemService)
        {
            _lessonRepository = lessonRepository;
            _courseService = courseService;
            _courseRepository = courseRepository;
            _mediaService = mediaService;
            _courseItemRepository = courseItemRepository;
            _azureService = azureService;
            _courseItemService = courseItemService;
        }     

        public async Task<int> Create(IFormCollection form, LessonDTO createLessonModel)
        {
            //string videoName = await _azureService.SaveInAsync(createLessonModel.VideoFile);
            //CourseItem newCourceItem = await _courseItemService.Create(createLessonModel);

            //Lesson newLesson = new Lesson()
            //{
            //    Id = Guid.NewGuid(),
            //    Theme = createLessonModel.Theme,
            //    Description = createLessonModel.Description,
            //    Body = createLessonModel.Body,
            //    VideoPath = videoName,
            //    CourseItemId = newCourceItem.Id,
            //    DateCreation = DateTime.Now.ToShortDateString()
            //};

            //await _lessonRepository.AddAsync(newLesson);

            //return newCourceItem.CourseId;

            return 0;
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
            var courseItem = _courseItemRepository.FirstOrDefault(y => y.Id == lesson.CourseItemId);
            var course = _courseRepository.FirstOrDefault(x => x.Id == courseItem.CourseId);

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
            var lesson = _lessonRepository.GetLessonByCourseItemId(courseItemId);

            if(lesson == null)
            {
                throw new Exception("Lesson not found!");
            }

            return new LessonDTO()
            {
                CourseItemId = courseItemId,
                Theme = lesson.Theme,
                Description = lesson.Description,
                Body = lesson.Body,
                VideoPath = lesson.VideoPath,
                DateCreation = lesson.DateCreation.ToString(),
                Id = lesson.Id
            };
        }

        public async Task<LessonDTO> Edit(LessonDTO lesson)
        {
            var currentLesson = _lessonRepository.FirstOrDefault(x => x.Id == lesson.Id);
            var currentCourse = _courseRepository.GetCourse(lesson.CourseID);

            if(currentLesson == null)
            {
                var countCurrentElements = _courseItemRepository.GetCourseItemsByCourseId(lesson.CourseID).Count();

                CourseItem newCourseItem = new CourseItem()
                {
                    TypeId = (int)e_ItemType.Lesson,
                    StatusId = (int)e_ItemStatus.OK,
                    DateCreation = DateTime.Now,
                    OrderNumber = countCurrentElements + 1,
                    CourseId = lesson.CourseID
                };

                string video = await _mediaService.SaveFile(lesson.VideoFile);

                Lesson newLesson = new Lesson()
                {
                    Theme = lesson.Theme,
                    Description = lesson.Description,
                    Body = lesson.Body,
                    VideoPath = video,
                    DateCreation = DateTime.Now.ToString(),
                    LoadVideoName = lesson.VideoFile.FileName
                };

                _courseItemRepository.Add(newCourseItem);

                newLesson.CourseItemId = newCourseItem.Id;

                _lessonRepository.Add(newLesson);

                lesson.Id = newLesson.Id;
                lesson.CourseItemId = newCourseItem.Id;
            }
            else
            {
                currentLesson.Theme = lesson.Theme;
                currentLesson.Body = lesson.Body;
                currentLesson.Description = lesson.Description;
                currentLesson.UpdateDate = DateTime.Now;

                if(currentLesson.LoadVideoName == null || currentLesson.LoadVideoName != lesson.VideoFile.FileName)
                {
                    await _mediaService.DeleteFile(currentLesson.VideoPath);
                    string video = await _mediaService.SaveFile(lesson.VideoFile);
                    currentLesson.LoadVideoName = lesson.VideoFile.FileName;
                }

                _lessonRepository.Update(currentLesson);
            }

            return lesson;
        }

        public void Delete(Guid id)
        {
            var lesson = _lessonRepository.FirstOrDefault(x => x.Id == id);
            var courseItem = _courseItemRepository.FirstOrDefault(x => x.Id == lesson.CourseItemId);
            courseItem.StatusId = (int)e_ItemStatus.DELETED;

            _courseItemRepository.Update(courseItem);   
        }
    }
}
