using Mentohub.Core.Infrastructure;
using Mentohub.Domain.Data.Entities.CourseEntities;

namespace Mentohub.Core.Repositories.Intefaces
{
    public interface ICourseItemRepository : ISingletoneService, IRepository<CourseItem>
    {
        public IQueryable<CourseItem> GetCourseItemsByCourseId(int courseId);

        public CourseItem GetCourseItemById(int? courseItemId);
    }
}
