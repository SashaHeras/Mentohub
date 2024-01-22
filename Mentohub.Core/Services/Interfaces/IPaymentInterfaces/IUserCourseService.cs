using Mentohub.Domain.Data.Entities;
using Mentohub.Domain.Data.Entities.CourseEntities;
using Mentohub.Domain.Data.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Interfaces.IPaymentInterfaces
{
    public interface IUserCourseService
    {

        ICollection<Course> GetUserCourses(string userId);
        ICollection<CurrentUser> GetUsers(int courseId);

    }
}
