using Mentohub.Domain.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Interfaces
{
    public interface IUserCourseService
    {
        ICollection<UserCourse> userCourses(string userId);
        ICollection<UserCourse> userCourses(int courseId);
        ICollection<CurrentUser> users(int courseId);
    }
}
