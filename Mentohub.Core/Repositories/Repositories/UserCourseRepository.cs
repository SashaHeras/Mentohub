using MassTransit;
using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Interfaces;
using Mentohub.Domain.Data.Entities;
using Mentohub.Domain.Data.Entities.CourseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Repositories.Repositories
{
    public class UserCourseRepository : Repository<UserCourse>,IUserCourseRepository
    {
        #pragma warning disable 8603
        private readonly ProjectContext _projectContext;
        public UserCourseRepository(ProjectContext projectContext) : base(projectContext)
        { 
            _projectContext= projectContext;
        }
        public UserCourse GetUserCourseById(int Id)
        {
            return GetAll().Where(u => u.Id == Id).FirstOrDefault();

        }

        public ICollection<UserCourse> GetUserCoursesByUserId(string userId)
        {
            return GetAll().Where(uc => uc.UserId == userId).ToList();
        }
    }
}
