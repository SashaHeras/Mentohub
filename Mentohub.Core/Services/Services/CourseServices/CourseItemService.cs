using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Interfaces.CourseInterfaces;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.Entities.CourseEntities;

namespace Mentohub.Core.Services.Services
{
    public class CourseItemService : ICourseItemService
    {
        private ProjectContext _projectContext;
        private readonly ICourseTypeRepository _courseItemTypeRepository;
        private readonly ICourseItemRepository _courseItemRepository;

        public CourseItemService(
            ICourseItemRepository courseItemRepository,
            ICourseTypeRepository courseTypeRepository,
            ProjectContext projectContext
            )
        {
            _courseItemTypeRepository = courseTypeRepository;
            _courseItemRepository = courseItemRepository;
            _projectContext = projectContext;
        }

        public CourseItem GetCourseItem(int id)
        {
            return _courseItemRepository.GetCourseItemById(id);
        }

        public async Task<CourseItem> Create(LessonDTO lesson)
        {
            var courseId = lesson.CourseID;
            var sameCourseItems = _courseItemRepository.GetCourseItemsByCourseId(courseId);
            var courseItemType = _courseItemTypeRepository.GetItemTypeByName("Lesson").Id;

            CourseItem newCourceItem = new CourseItem()
            {
                CourseId = Convert.ToInt32(lesson.CourseID),
                DateCreation = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
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

        public async Task<CourseItem> DeleteCourseItem(int courseItemId, int typeId)
        {
            var item = GetCourseItem(courseItemId);
            item.StatusId = GetStatusID("Deleted");

            await UpdateCourseItem(item);
            return item;
        }

        /// <summary>
        /// Return IQuerable of course items
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public IQueryable<CourseItem> GetElementsByCourseId(int courseId)
        {
            return _courseItemRepository.GetCourseItemsByCourseId(courseId);
        }

        private int GetStatusID(string name)
        {
            var status = _projectContext.ItemsStatuses.Where(i => i.Name == name).FirstOrDefault();
            if (status == null)
            {
                return -1;
            }

            return status.Id;
        }
    }
}
