using Mentohub.Core.Repositories.Interfaces.PaymentInterfaces;
using Mentohub.Core.Services.Interfaces.IPaymentInterfaces;
using Mentohub.Domain.Data.Entities;
using Mentohub.Domain.Data.Entities.CourseEntities;
using Mentohub.Domain.Data.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Services.PaymentServices
{
    public class UserCourseService : IUserCourseService
    {
#pragma warning disable 8603
        private readonly IUserCourseRepository _courseRepository;
        public UserCourseService(IUserCourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public IUserCourseService CreateUserCourse(int courseId, string userId)
        {
            var userCourse = _courseRepository.AddUserCourse(courseId, userId);
            
            return (IUserCourseService)userCourse;
        }

        public bool DeleteUserCourse(int id)
        {
            var usercourse=_courseRepository.GetUserCourseById(id);
            if(usercourse == null) 
            {
                return false;
                throw new ArgumentNullException(nameof(UserCourse), "The UserCourse does not exist");
            }
            _courseRepository.DeleteUserCourse(usercourse);
            return true;
        }

        public UserCourse GetUserCourse(int id)
        {
            var usercourse = _courseRepository.GetUserCourseById(id);
            if (usercourse == null)
            {
                throw new ArgumentNullException(nameof(UserCourse), "The UserCourse does not exist");
            }
            return usercourse;
        }

        /// <summary>
        /// обираємо всі курси, які придбав користувач
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public ICollection<Course> GetUserCourses(string userId)
        {
            var userCourses = _courseRepository.GetUserCoursesByUserId(userId).ToList();
            if (userCourses == null)
            {
                throw new ArgumentNullException(nameof(userCourses), "The collection cannot be null.");
            }
            return userCourses.Select(u => u.Course).ToList();
        }
        /// <summary>
        /// обираємо всіх користувачів, які придбали певний курс
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public ICollection<CurrentUser> GetUsers(int courseId)
        {
            var userCourses = _courseRepository.GetUserCoursesByCourseId(courseId).ToList();
            if (userCourses == null)
            {
                throw new ArgumentNullException(nameof(userCourses), "The collection cannot be null.");
            }
            return userCourses.Select(cu => cu.СurrentUser).ToList();
        }

        public Task<bool> UpdateUserCourse(int id)
        {
            throw new NotImplementedException();
        }
    }
}
