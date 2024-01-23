using Mentohub.Core.Infrastructure;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Domain.Data.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Repositories.Interfaces.PaymentInterfaces
{
    public interface IUserCourseRepository : ISingletoneService, IRepository<UserCourse>
    {
        void UpdateUserCourse(UserCourse userCourse);

        UserCourse GetUserCourseById(int Id);

        void DeleteUserCourse(UserCourse userCourse);

        ICollection<UserCourse> GetUserCoursesByUserId(string userId);

        ICollection<UserCourse> GetUserCoursesByCourseId(int courseId);

    }
}
