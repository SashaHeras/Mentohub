using Mentohub.Core.AllExceptions;
using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Core.Repositories.Interfaces;
using Mentohub.Core.Repositories.Interfaces.CourseInterfaces;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Core.Services.Services;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.Entities;
using Mentohub.Domain.Data.Entities.CourseEntities;
using Mentohub.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
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
        Mock<ProjectContext> _context = new Mock<ProjectContext>();
        Mock< ILessonRepository> _lessonRepository=new Mock<ILessonRepository>();
        Mock< ICourseService> _courseService=new Mock<ICourseService>();
        Mock< IMediaService> _mediaService=new Mock<IMediaService>();
        Mock< IAzureService> _azureService=new Mock<IAzureService>();
        Mock< ICourseItemService> _courseItemService=new Mock<ICourseItemService>();

        Mock< ICourseRepository> _courseRepository=new Mock<ICourseRepository>();
        Mock< ICourseItemRepository> _courseItemRepository=new Mock<ICourseItemRepository>();
        Mock< ICourseItemTypeRepository> _courseItemTypeRepository=new Mock<ICourseItemTypeRepository>();
        Mock< ICourseBlockRepository> _courseBlockRepository=new Mock<ICourseBlockRepository>();
        public LessonServiceTests() 
        {
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
            // Arrange
            var formFiles = new FormFileCollection();
            formFiles.Add(new FormFile(Stream.Null, 0, 0, "file", "video.mp4"));
            var form = new FormCollection(new Dictionary<string, StringValues>(), formFiles);

            var lesson = new Lesson
            {
                VideoPath = "old_video_path.mp4",
                CourseItemId = 123 // Sample course item ID
                                   // Initialize other properties as needed
            };

            var courseId = 456; // Sample course ID

            _azureService.Setup(mock => mock.DeleteFromAzure(lesson.VideoPath)).Returns((Task<bool>)Task.CompletedTask);
            _azureService.Setup(mock => mock.SaveInAsync(It.IsAny<IFormFile>())).ReturnsAsync("new_video_path.mp4");

            _courseItemService.Setup(mock => mock.GetCourseItem(lesson.CourseItemId)).Returns(new CourseItem
            {
                // Mocking the existing course item
                CourseId = courseId
            });

            _courseItemService.Setup(mock => mock.UpdateCourseItem(It.IsAny<CourseItem>())).Returns((Task<CourseItem>)Task.CompletedTask);

            _mediaService.Setup(mock => mock.DeleteMediaFromProject(It.IsAny<IFormFile>()));
            // Act
            var result = await _lessonService.Edit(form, lesson);
            // Assert
            Assert.Equal(courseId, result);
        }
    }
}
