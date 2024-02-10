using Mentohub.Core.Infrastructure;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Domain.Data.Entities.CourseEntities;

namespace Mentohub.Core.Repositories.Interfaces.CourseInterfaces
{
    public interface ICourseItemTypeRepository : ISingletoneService, IRepository<CourseItemType>
    {

    }
}
