using Mentohub.Domain.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Repositories.Interfaces
{
    public interface IUserCourseRepository
    {
        public UserCourse GetUserCourseById(int Id);

        public ICollection< UserCourse> GetUserCoursesByUserId(string userId);

    }
}
