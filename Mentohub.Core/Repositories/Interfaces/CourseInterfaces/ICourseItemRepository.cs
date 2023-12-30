using Mentohub.Core.Infrastructure;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Domain.Data.Entities.CourseEntities;

namespace Mentohub.Core.Repositories.Interfaces.CourseInterfaces
{
    public interface ICourseItemRepository : ISingletoneService, IRepository<CourseItem>
    {
        public IQueryable<CourseItem> GetCourseItemsByCourseId(int courseId);

        public CourseItem GetCourseItemById(int? courseItemId);
    }
}
