using Mentohub.Core.Infrastructure;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Domain.Data.Entities.CourseEntities;

namespace Mentohub.Core.Repositories.Interfaces.CourseInterfaces
{
    public interface ICourseTypeRepository : ISingletoneService, IRepository<CourseItemType>
    {
        public CourseItemType GetTypeById(int id);

        public CourseItemType GetItemTypeByName(string name);
    }
}
