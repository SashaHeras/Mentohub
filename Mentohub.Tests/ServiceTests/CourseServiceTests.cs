using Mentohub.Core.Repositories.Interfaces;
using Mentohub.Core.Repositories.Interfaces.CourseInterfaces;
using Mentohub.Core.Repositories.Repositories.CourseRepositories;
using Mentohub.Core.Repositories.Repositories;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Core.Services.Services;
using Mentohub.Domain.Data.DTO.CourseDTOs;
using Mentohub.Domain.Data.Entities.CourseEntities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Tests.ServiceTests
{
    public class CourseServiceTests
    {
        private readonly Mock<CourseService> _courseService = new Mock<CourseService>();
        Mock<ICourseRepository> _courseRepository = new Mock<ICourseRepository>();
        Mock<ICourseLanguageRepository> _courseLanguageRepository = new Mock<ICourseLanguageRepository>();
        Mock<ICourseLevelRepository> _courseLevelRepository = new Mock<ICourseLevelRepository>();
        Mock<ICourseItemRepository> _courseItemRepository=new Mock<ICourseItemRepository>();
        Mock<ITagRepository> _tagRepository=new Mock<ITagRepository>();
        Mock<ICourseTagRepository> _courseTagRepository=new Mock<ICourseTagRepository>();
        Mock<ISubjectRepository> _subjectRepository=new Mock<ISubjectRepository>();
        Mock<ICommentRepository> _commentRepository=new Mock<ICommentRepository>();
        Mock<ICourseBlockRepository> _courseBlockRepository=new Mock<ICourseBlockRepository>();
        Mock<ICRUD_UserRepository> _cRUD_UserRepository=new Mock<ICRUD_UserRepository>();

        Mock<IMediaService >_mediaService=new Mock<IMediaService>();
        Mock<ICourseViewService> _courseViewsService=new Mock<ICourseViewService>();
        Mock<ICourseSubjectService>_courseSubjectService=new Mock<ICourseSubjectService>();
        Mock<ICourseLanguageService> _courseLanguageService=new Mock<ICourseLanguageService>();
        Mock<ICourseLevelService> _courseLevelService=new Mock<ICourseLevelService>();
       

        //public CourseServiceTests()
        //{
        //    _courseService = new CourseService(
            
        //    _courseRepository.Object,
        //    _courseItemRepository.Object,
        //    _commentRepository.Object, 
        //    _tagRepository.Object,
        //    _subjectRepository.Object,
        //    _courseViewsService.Object,
        //    _courseLanguageRepository.Object,
        //    _mediaService.Object,
        //    _courseTagRepository.Object,
        //    _courseLevelRepository.Object,
        //    _courseSubjectService.Object,
        //    _courseLanguageService.Object,
        //    _courseBlockRepository.Object,          
        //    _courseLevelService.Object,
        //    _cRUD_UserRepository.Object,
        //    );
        //}

        //[Fact]
        //public async Task Apply_Creates_New_Course_When_Not_Exists()
        //{
        //    // Arrange
        //    var courseDTO = new CourseDTO
        //    {
        //        // Initialize properties as needed for testing
        //    };

        //    _courseLanguageRepository.Setup(x => x.FindById(It.IsAny<int>())).Returns(new CourseLanguage());
        //    _courseLevelRepository.Setup(x => x.FindById(It.IsAny<int>())).Returns(new CourseLevel());

        //    _courseRepository.Setup(x => x.FirstOrDefault(It.IsAny<Expression<Func<Course, bool>>>()))
        //                         .Returns((Course)null); // Simulate that the course does not exist

        //    // Act
        //    var result = await _courseService.Apply(courseDTO);

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Equal(courseDTO.Id, result.Id);
        //    // Add more assertions as needed
        //}

        //[Fact]
        //public async Task Apply_Updates_Existing_Course_When_Exists()
        //{
        //    // Arrange
        //    var existingCourse = new Course { Id = 1 }; // Mock existing course
        //    var courseDTO = new CourseDTO { Id = 1 }; // DTO corresponding to the existing course

        //    _courseLanguageRepository.Setup(x => x.FindById(It.IsAny<int>())).Returns(new CourseLanguage());
        //    _courseLevelRepository.Setup(x => x.FindById(It.IsAny<int>())).Returns(new CourseLevel());

        //    _courseRepository.Setup(x => x.FirstOrDefault(It.IsAny<Expression<Func<Course, bool>>>()))
        //                         .Returns(existingCourse); // Simulate that the course exists

        //    // Act
        //    var result = await _courseService.Apply(courseDTO);

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Equal(courseDTO.Id, result.Id);
        //    // Add more assertions as needed
        //}
    }
}
