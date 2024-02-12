using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Core.Repositories.Interfaces.CourseInterfaces;
using Mentohub.Core.Repositories.Repositories.CourseRepositories;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.Entities.CourseEntities;
using Mentohub.Domain.Data.Enums;
using Mentohub.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Mentohub.Core.Services.Services
{
    public class LessonService : ILessonService
    {
        private readonly ProjectContext _context;
        private readonly ILessonRepository _lessonRepository;
        private readonly ICourseService _courseService;
        private readonly IMediaService _mediaService;
        private readonly IAzureService _azureService;
        private readonly ICourseItemService _courseItemService;

        private readonly ICourseRepository _courseRepository;
        private readonly ICourseItemRepository _courseItemRepository;
        private readonly ICourseItemTypeRepository _courseItemTypeRepository;
        private readonly ICourseBlockRepository _courseBlockRepository;

        public LessonService(
            ProjectContext context,
            ILessonRepository lessonRepository,
            ICourseService courseService,
            IMediaService mediaService,
            ICourseRepository courseRepository,
            ICourseItemRepository courseItemRepository,
            IAzureService azureService,
            ICourseItemService courseItemService,
            ICourseItemTypeRepository courseItemTypeRepository,
            ICourseBlockRepository courseBlockRepository)
        {
            _lessonRepository = lessonRepository;
            _courseService = courseService;
            _courseRepository = courseRepository;
            _context = context;
            _mediaService = mediaService;
            _courseItemRepository = courseItemRepository;
            _azureService = azureService;
            _courseItemService = courseItemService;
            _courseBlockRepository = courseBlockRepository;
            _courseItemTypeRepository = courseItemTypeRepository;
        }

        public async Task<int> Edit(IFormCollection form, Lesson lesson)
        {
            if (form.Files.Count != 0)
            {
                await _azureService.DeleteFromAzure(lesson.VideoPath);
                lesson.VideoPath = await _azureService.SaveInAsync(form.Files[0]);
            }

            CourseItem currentCourseItem = _courseItemService.GetCourseItem(lesson.CourseItemId);
            currentCourseItem.DateCreation = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            await _courseItemService.UpdateCourseItem(currentCourseItem);

            int courseId = currentCourseItem.CourseId;

            UpdateLesson(lesson);

            _mediaService.DeleteMediaFromProject(form.Files[0]);

            return courseId;
        }

        public LessonDTO GetLesson(Guid id)
        {           
            var lesson = _lessonRepository.GetLessonById(id);
            var courseItem = _courseItemRepository.FirstOrDefault(y => y.id == lesson.CourseItemId);
            var course = _courseRepository.FirstOrDefault(x => x.Id == courseItem.CourseId);

            LessonDTO result = new LessonDTO()
            {
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
            currentLesson.DateCreation = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc).ToShortDateString();

            await _lessonRepository.UpdateAsync(currentLesson);
        }

        public LessonDTO GetLessonByCourseItem(int courseItemId)
        {
            var lesson = _lessonRepository.GetLessonByCourseItemId(courseItemId);

            if (lesson == null)
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

        public async Task<LessonDTO> Apply(LessonDTO lesson)
        {
            var currentLesson = _lessonRepository.FirstOrDefault(x => x.Id == lesson.Id);
            var currentCourse = _courseRepository.GetCourse(lesson.CourseID);
            var block = _courseBlockRepository.GetById(lesson.CourseBlockID);
            if(block == null)
            {
                throw new Exception("Unknown block!");
            }

            if (currentLesson == null)
            {
                try
                {
                    var countCurrentElements = _courseItemRepository.GetCourseItemsByCourseId(lesson.CourseID).Count();

                    CourseItem newCourseItem = new CourseItem()
                    {
                        StatusId = (int)e_ItemStatus.OK,
                        DateCreation = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                        OrderNumber = countCurrentElements + 1,
                        CourseId = lesson.CourseID,
                        CourseBlockID = block.ID
                    };

                    lesson.VideoPath = await _mediaService.SaveFile(lesson.VideoFile);

                    Lesson newLesson = LessonFromDTO(lesson);

                    _courseItemRepository.Add(newCourseItem);

                    newLesson.CourseItemId = newCourseItem.id;

                    _lessonRepository.Add(newLesson);

                    lesson.Id = newLesson.Id;
                    lesson.CourseItemId = newCourseItem.id;
                }
                catch(Exception ex)
                {
                    if (lesson.VideoPath != null)
                    {
                        await _mediaService.DeleteFile(lesson.VideoPath);
                    }

                    throw;
                }
            }
            else
            {
                currentLesson.Theme = lesson.Theme;
                currentLesson.Body = lesson.Body;
                currentLesson.Description = lesson.Description;
                currentLesson.UpdateDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

                if (currentLesson.LoadVideoName == null || currentLesson.LoadVideoName != lesson.VideoFile.FileName)
                {
                    await _mediaService.DeleteFile(currentLesson.VideoPath);
                    lesson.VideoPath = await _mediaService.SaveFile(lesson.VideoFile);
                    currentLesson.VideoPath = lesson.VideoPath;
                    currentLesson.LoadVideoName = lesson.VideoFile.FileName;
                }

                _lessonRepository.Update(currentLesson);
            }

            return lesson;
        }

        public Lesson LessonFromDTO(LessonDTO data)
        {
            return new Lesson()
            {
                Theme = data.Theme,
                Description = data.Description,
                Body = data.Body,
                VideoPath = data.VideoPath,
                DateCreation = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc).ToString(),
                LoadVideoName = data.VideoFile.FileName
            };
        }

        public async Task Delete(Guid id)
        {
            var lesson = _lessonRepository.FirstOrDefault(x => x.Id == id);
            var courseItem = _courseItemRepository.FirstOrDefault(x => x.id == lesson.CourseItemId);

            await _azureService.DeleteFromAzure(lesson.VideoPath);

            _context.Lessons.Remove(lesson);
            _context.CourseItem.Remove(courseItem);
        }

        
    }
}
