using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Domain.Entities;

namespace Mentohub.Core.Repositories.Repositories
{
    public class CourseItemRepository : Repository<CourseItem>, ICourseItemRepository
    {
        private readonly ProjectContext _context;

        public CourseItemRepository(ProjectContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }

        /// <summary>
        /// Method return CourseItem by id
        /// </summary>
        /// <param name="courseItemId"></param>
        /// <returns></returns>
        public CourseItem GetCourseItemById(int? courseItemId)
        {
            return GetAll().Where(ci => ci.Id == courseItemId).FirstOrDefault();
        }

        /// <summary>
        /// Method return collection of CourseItem with same course id
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public IQueryable<CourseItem> GetCourseItemsByCourseId(int courseId)
        {
            return GetAll().Where(ci => ci.CourseId == courseId).OrderBy(ci => ci.OrderNumber);
        }
    }
}
