using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Interfaces.CourseInterfaces;
using Mentohub.Domain.Data.Entities.CourseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Repositories.Repositories.CourseRepositories
{
    public class CourseViewsRepository : Repository<CourseViews>, ICourseViewsRepository
    {
        public CourseViewsRepository(ProjectContext repositoryContext) : base(repositoryContext)
        {

        }
    }
}
