using Mentohub.Core.Repositories.Interfaces.PaymentInterfaces;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.DTO.Payment;
using Mentohub.Domain.Data.Entities;
using Mentohub.Domain.Data.Entities.CourseEntities;
using Mentohub.Domain.Data.Entities.Order;

namespace Mentohub.Core.Services.Services.PaymentServices
{
    public class UserCourseService : IUserCourseService
    {
        #pragma warning disable 8603
        private readonly IUserCourseRepository _userCourseRepository;

        public UserCourseService(IUserCourseRepository userCourseRepository)
        {
            _userCourseRepository = userCourseRepository;
        }

        public UserCourse CreateUserCourse(UserCourseDTO data)
        {
            var userCourse = new UserCourse()
            {
                Created = DateTime.Now,
                CourseId = data.CourseId,
                UserId = data.UserId,
                OrderItemId = data.OrderItemId,
                OrderPaymentId = data.OrderPaymentId
            };

            _userCourseRepository.Add(userCourse);

            return userCourse;
        }

        public bool DeleteUserCourse(int id)
        {
            var usercourse = _userCourseRepository.GetUserCourseById(id);
            if(usercourse == null) 
            {
                throw new ArgumentNullException(nameof(UserCourse), "The UserCourse does not exist");
            }

            _userCourseRepository.DeleteUserCourse(usercourse);
            return true;
        }

        public UserCourse GetUserCourse(int id)
        {
            var usercourse = _userCourseRepository.GetUserCourseById(id);
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
        public ICollection<UserCourseDTO> GetUserCourses(string userId)
        {
            var userCourses = _userCourseRepository.GetUserCoursesByUserId(userId).ToList();
            if (userCourses == null)
            {
                throw new ArgumentNullException(nameof(userCourses), "The collection cannot be null.");
            }
            return userCourses;
        }

        /// <summary>
        /// обираємо всіх користувачів, які придбали певний курс
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public ICollection<UserDTO> GetUsers(int courseId)
        {
            var userCourses = _userCourseRepository.GetUserCoursesByCourseId(courseId).ToList();
            if (userCourses == null)
            {
                throw new ArgumentNullException(nameof(userCourses), "The collection cannot be null.");
            }

            return userCourses.Select(cu => new UserDTO()
            {
                Id = cu.СurrentUser.Id,
                Email = cu.СurrentUser.Email
            }).ToList();
        }

        public Task<bool> UpdateUserCourse(int id)
        {
            throw new NotImplementedException();
        }
    }
}
