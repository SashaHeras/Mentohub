using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Intefaces;
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
    public class CourseBlockRepository : Repository<CourseBlock>, ICourseBlockRepository
    {
        #pragma warning disable 8603
        public CourseBlockRepository(ProjectContext repositoryContext) : base(repositoryContext)
        {

        }

        public CourseBlock GetById(int id)
        {
            return GetAll()
                   .Where(x => x.ID == id)
                   .Include(x => x.Course)
                   .Include(x => x.CourseItems)
                   .FirstOrDefault();
        }

        public IQueryable<CourseBlock> GetCourseBlocks(int courseID)
        {
            return GetAll()
                   .Where(x => x.CourseID == courseID)
                   .Include(x => x.Course)
                   .Include(x => x.CourseItems);
        }
    }
}
