using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Core.Repositories.Interfaces.CourseInterfaces;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.Data.DTO.CourseDTOs;
using Mentohub.Domain.Data.Entities.CourseEntities;
using Mentohub.Domain.Mappers;

namespace Mentohub.Core.Services.Services
{
    public class CourseBlockService : ICourseBlockService
    {
        private readonly ICourseBlockRepository _courseBlockRepository;
        private readonly ICourseItemRepository _courseItemRepository;
        private readonly ILessonService _lessonService;
        private readonly ITestService _testService;
        private readonly ICourseRepository _courseRepository;

        public CourseBlockService(
            ICourseBlockRepository courseBlockRepository,
            ICourseRepository courseRepository,
            ICourseItemRepository courseItemRepository,
            ITestService testService,
            ILessonService lessonService
        )
        {
            _courseBlockRepository = courseBlockRepository;
            _courseRepository = courseRepository;
            _courseItemRepository = courseItemRepository;
            _lessonService = lessonService;
            _testService = testService;
        }

        public CourseBlockDTO Edit(CourseBlockDTO data)
        {
            var course = _courseRepository.FirstOrDefault(x => x.Id == data.CourseID);
            if (course == null)
            {
                throw new Exception("Unnown course!");
            }

            int courseBlocksCnt = course.CourseBlocks?.Count() ?? 0;

            var block = _courseBlockRepository.GetById(data.ID);
            if (block == null)
            {
                block = new CourseBlock()
                {
                    CourseID = data.CourseID,
                    Name = data.Name,
                    OrderNumber = courseBlocksCnt + 1,
                    CourseItems = new List<CourseItem>()
                };

                _courseBlockRepository.Add(block);
            }
            else
            {
                block.OrderNumber = block.OrderNumber;
                block.Name = data.Name;

                _courseBlockRepository.Update(block);
            }

            data = CourseMapper.ToDTO(block);

            return data;
        }

        public async Task Delete(int ID)
        {
            var block = _courseBlockRepository.FirstOrDefault(x => x.ID == ID);
            var items = block.CourseItems.ToList();

            foreach(var item in items)
            {
                if(item.Lesson != null)
                {
                    await _lessonService.Delete(item.Lesson.Id);
                }
                else if(item.Test != null) {
                    _testService.Delete(item.Test.Id);
                }
            }

            _courseBlockRepository.Delete(block);
        }
    }
}
