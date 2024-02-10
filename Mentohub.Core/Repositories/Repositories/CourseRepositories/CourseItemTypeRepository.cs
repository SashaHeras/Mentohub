using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Interfaces.CourseInterfaces;
using Mentohub.Domain.Data.Entities.CourseEntities;

namespace Mentohub.Core.Repositories.Repositories.CourseRepositories
{
    public class CourseItemTypeRepository : Repository<CourseItemType>, ICourseItemTypeRepository
    {
        public CourseItemTypeRepository(ProjectContext repositoryContext) : base(repositoryContext)
        {

        }
    }
}
