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
        UserCourse GetUserCourseById(int Id);

        ICollection<UserCourse> GetUserCoursesByUserId(string userId);

        ICollection<UserCourse> GetUserCoursesByCourseId(int courseId);

    }
}
