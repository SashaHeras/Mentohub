
using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Core.Repositories.Interfaces.CourseInterfaces;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Core.Services.Services;
using Mentohub.Domain.Data.Entities.CourseEntities;
using Mentohub.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Tests.ServiceTests
{
    public class LessonServiceTests
    {
        ILessonService _lessonService;
        Mock<ProjectContext> _context;
        Mock<ILessonRepository> _lessonRepository;
        Mock<ICourseService> _courseService;
        Mock<IMediaService> _mediaService;
        Mock<IAzureService> _azureService;
        Mock<ICourseItemService> _courseItemService;

        Mock<ICourseRepository> _courseRepository;
        Mock<ICourseItemRepository> _courseItemRepository;
        Mock<ICourseItemTypeRepository> _courseItemTypeRepository;
        Mock<ICourseBlockRepository> _courseBlockRepository;

        public LessonServiceTests()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProjectContext>();
            var options = optionsBuilder.Options;
            _context = new Mock<ProjectContext>(options);
            _lessonRepository = new Mock<ILessonRepository>();
            _courseService = new Mock<ICourseService>();
            _mediaService = new Mock<IMediaService>();
            _azureService = new Mock<IAzureService>();
            _courseItemService = new Mock<ICourseItemService>();
            _courseRepository = new Mock<ICourseRepository>();
            _courseItemRepository = new Mock<ICourseItemRepository>();
            _courseItemTypeRepository = new Mock<ICourseItemTypeRepository>();
            _courseBlockRepository = new Mock<ICourseBlockRepository>();

            _lessonService = new LessonService(
                _context.Object,
                _lessonRepository.Object,
                _courseService.Object,
                _mediaService.Object,
                _courseRepository.Object,
                _courseItemRepository.Object,
                _azureService.Object,
                _courseItemService.Object,
                _courseItemTypeRepository.Object,
                _courseBlockRepository.Object
            );
        }

        [Fact]
        public async Task Edit_Returns_Correct_CourseId()
        {
            var formFiles = new FormFileCollection();
            formFiles.Add(new FormFile(Stream.Null, 0, 0, "file", "video.mp4"));
            var form = new FormCollection(new Dictionary<string, StringValues>(), formFiles);

            var lesson = new Lesson
            {
                VideoPath = "old_video_path.mp4",
                CourseItemId = 123 
            };

            var courseId = 456; 

            _azureService.Setup(mock => mock.DeleteFromAzure(lesson.VideoPath)).ReturnsAsync(true);
            _azureService.Setup(mock => mock.SaveInAsync(It.IsAny<IFormFile>())).ReturnsAsync("new_video_path.mp4");

            _courseItemService.Setup(mock => mock.GetCourseItem(lesson.CourseItemId)).Returns(new CourseItem
            {
                CourseId = courseId
            });

            _azureService.Setup(mock => mock.DeleteFromAzure(lesson.VideoPath)).ReturnsAsync(true);
           _mediaService.Setup(mock => mock.DeleteMediaFromProject(It.IsAny<IFormFile>()));
            // Act
            var result = await _lessonService.Edit(form, lesson);
            // Assert
            Assert.Equal(courseId, result);
        }
        [Fact]
        public async Task UpdateLesson_Should_Update_Lesson()
        {
            var newLesson = new Lesson
            {
                Id = Guid.NewGuid(),
                Theme = "New Theme",
                Description = "New Description",
                Body = "New Body",
                CourseItemId = 123,
                DateCreation = DateTime.UtcNow.ToShortDateString() 
            };
            var currentLesson = new Lesson
            {
                Id = Guid.NewGuid(),
                Theme = "Old Theme",
                Description = "Old Description",
                Body = "Old Body",
                CourseItemId = 123,
                DateCreation = DateTime.UtcNow.ToShortDateString() 
            };

            _lessonRepository.Setup(repo => repo.GetLessonById(newLesson.Id)).Returns(currentLesson);
            _lessonRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Lesson>())).ReturnsAsync(newLesson);

            // Act
            _lessonService.UpdateLesson(newLesson);

            // Assert
            Assert.Equal(newLesson.Theme, currentLesson.Theme);
            Assert.Equal(newLesson.Description, currentLesson.Description);
            Assert.Equal(newLesson.Body, currentLesson.Body);
            Assert.Equal(newLesson.CourseItemId, currentLesson.CourseItemId);
        }
    }

}

