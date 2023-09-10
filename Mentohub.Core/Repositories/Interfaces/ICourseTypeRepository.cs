using Mentohub.Core.Infrastructure;
using Mentohub.Domain.Entities;

namespace Mentohub.Core.Repositories.Intefaces
{
    public interface ICourseTypeRepository : ISingletoneService, IRepository<CourseItemType>
    {
        public CourseItemType GetTypeById(int id);

        public CourseItemType GetItemTypeByName(string name);
    }
}
