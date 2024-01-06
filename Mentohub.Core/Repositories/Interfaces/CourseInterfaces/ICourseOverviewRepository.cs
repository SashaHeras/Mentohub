using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Domain.Data.Entities.CourseEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Repositories.Interfaces.CourseInterfaces
{
    public interface ICourseOverviewRepository : IRepository<CourseOverview>
    {
        public CourseOverview FindById(int id);

        public IQueryable<CourseOverview> GetCourseOverviews(int courseID);
    }
}
