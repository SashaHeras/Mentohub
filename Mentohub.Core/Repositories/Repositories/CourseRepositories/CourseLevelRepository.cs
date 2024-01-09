using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Core.Repositories.Interfaces.CourseInterfaces;
using Mentohub.Domain.Data.Entities.CourseEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Repositories.Repositories.CourseRepositories
{
    public class CourseLevelRepository : Repository<CourseLevel>, ICourseLevelRepository
    {
        #pragma warning disable 8603
        public CourseLevelRepository(ProjectContext repositoryContext) : base(repositoryContext)
        {

        }

        public CourseLevel FindById(int id)
        {
            return GetAll(x => x.ID == id)
                   .FirstOrDefault();
        }
    }
}
