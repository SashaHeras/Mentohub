using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Domain.Data.Entities.CourseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Repositories.Interfaces.CourseInterfaces
{
    public interface ICourseBlockRepository : IRepository<CourseBlock>
    {
        public CourseBlock GetById(int id);

        public IQueryable<CourseBlock> GetCourseBlocks(int courseID);

        public void Delete(CourseBlock block);
    }
}
