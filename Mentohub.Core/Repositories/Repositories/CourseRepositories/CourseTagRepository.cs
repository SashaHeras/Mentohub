using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Interfaces.CourseInterfaces;
using Mentohub.Domain.Data.Entities.CourseEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Repositories.Repositories.CourseRepositories
{
    public class CourseTagRepository : Repository<CourseTag>, ICourseTagRepository
    {
        #pragma warning disable 8603
        public CourseTagRepository(ProjectContext repositoryContext) : base(repositoryContext)
        {

        }

        public CourseTag GetById(Guid id)
        {
            return GetAll().Where(x => x.ID == id)
                           .Include(x => x.Course)
                           .Include(x => x.Tag)
                           .FirstOrDefault();
        }
    }
}
