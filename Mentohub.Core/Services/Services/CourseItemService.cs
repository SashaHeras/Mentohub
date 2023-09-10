using Mentohub.Core.Repositories.Repositories;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Entities;

namespace Mentohub.Core.Services.Services
{
    public class CourseItemService
    {
        private CourseTypeRepository _courseItemTypeRepository;
        private CourseItemRepository _courseItemRepository;

        public CourseItemService(CourseItemRepository courseItemRepository, CourseTypeRepository courseTypeRepository) {
            _courseItemTypeRepository = courseTypeRepository;
            _courseItemRepository = courseItemRepository;
        }

        public CourseItem GetCourseItem(int id)
        {
            return _courseItemRepository.GetCourseItemById(id);
        }

        public async Task<CourseItem> CreateNewCourseItem(LessonDTO lesson)
        {
            var courseId = lesson.CourseID;
            var sameCourseItems = _courseItemRepository.GetCourseItemsByCourseId(courseId);
            var courseItemType = _courseItemTypeRepository.GetItemTypeByName("Lesson").Id;

            CourseItem newCourceItem = new CourseItem()
            {
                TypeId = courseItemType,
                CourseId = Convert.ToInt32(lesson.CourseID),
                DateCreation = DateTime.Now,
                OrderNumber = sameCourseItems.Count() > 0 ? sameCourseItems.Last().OrderNumber + 1 : 1,
                StatusId = 2
            };

            await _courseItemRepository.AddAsync(newCourceItem);

            return newCourceItem;
        }

        public IQueryable<CourseItem> GetCourseItems(int courseId)
        {
            return _courseItemRepository.GetCourseItemsByCourseId(courseId).OrderBy(ci => ci.OrderNumber);
        }

        public CourseItemType GetItemTypeByName(string name)
        {
            return _courseItemTypeRepository.GetItemTypeByName(name);
        }

        public CourseItemType GetItemType(int id)
        {
            return _courseItemTypeRepository.GetTypeById(id);
        }

        public async Task<CourseItem> UpdateCourseItem(CourseItem courseItem)
        {
            return await _courseItemRepository.UpdateAsync(courseItem);
        }

        /// <summary>
        /// Return IQuerable of course items
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public IQueryable<CourseItem> GetElementsByCourseId(int courseId)
        {
            return (IQueryable<CourseItem>)_courseItemRepository.GetCourseItemsByCourseId(courseId);
        }
    }
}
