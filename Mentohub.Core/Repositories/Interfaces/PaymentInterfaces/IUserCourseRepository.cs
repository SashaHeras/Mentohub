using Mentohub.Domain.Data.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Repositories.Interfaces.PaymentInterfaces
{
    public interface IUserCourseRepository
    {
        UserCourse AddUserCourse(int courseId, string userId);
        void UpdateUserCourse(UserCourse userCourse);
        void DeleteUserCourse(UserCourse userCourse);
        UserCourse GetUserCourseById(int Id);

        ICollection<UserCourse> GetUserCoursesByUserId(string userId);

        ICollection<UserCourse> GetUserCoursesByCourseId(int courseId);

    }
}
