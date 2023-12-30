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
    public class CourseOverviewRepository : Repository<CourseOverview>, ICourseOverviewRepository
    {
        #pragma warning disable 8603
        public CourseOverviewRepository(ProjectContext repositoryContext) : base(repositoryContext)
        {

        }

        public CourseOverview FindById(int id)
        {
            return GetAll().Where(x => x.ID == id)
                           .Include(x => x.Course)
                           .FirstOrDefault();
        }

        public IQueryable<CourseOverview> GetCourseOverviews(int courseID)
        {
            return GetAll().Where(x => x.CourseID == courseID)
                           .Include(x => x.Course);
        }
    }
}
