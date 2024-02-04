using Mentohub.Core.Repositories.Interfaces;
using Mentohub.Core.Repositories.Interfaces.CourseInterfaces;
using Mentohub.Core.Repositories.Interfaces.PaymentInterfaces;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.DTO.CourseDTOs;
using Mentohub.Domain.Data.DTO.Payment;
using Mentohub.Domain.Data.Entities;
using Mentohub.Domain.Data.Entities.CourseEntities;
using Mentohub.Domain.Data.Entities.Order;
using Mentohub.Domain.Helpers;
using Mentohub.Domain.Mappers;

namespace Mentohub.Core.Services.Services.PaymentServices
{
    public class UserCourseService : IUserCourseService
    {
        #pragma warning disable 8603
        private readonly IUserCourseRepository _userCourseRepository;
        private readonly ICRUD_UserRepository _cRUD_UserRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IOrderRepository _orderRepository;
        public UserCourseService(IUserCourseRepository userCourseRepository,
            ICRUD_UserRepository cRUD_UserRepository,
            ICourseRepository courseRepository,
            IOrderRepository orderRepository)
        {
            _userCourseRepository = userCourseRepository;
            _cRUD_UserRepository= cRUD_UserRepository;
            _courseRepository = courseRepository;
            _orderRepository = orderRepository;
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
        public ICollection<CourseDTO> GetUserCourses(string userId)
        {
            var userCourses = _userCourseRepository.GetUserCoursesByUserId(userId);
            var orders=_orderRepository.GetAll(o => o.UserID == userId);
            if (orders.Count()==0 && userCourses.Count==0)
            {
                throw new ArgumentNullException(nameof(userCourses), "This user has not purchased any courses.");
            }
            var CoursesDTO = new List<CourseDTO>();
            foreach (var item in userCourses)
            {
                var course = _courseRepository.GetCourse(item.CourseId);
                CoursesDTO.Add(CourseMapper.ToDTO(course));               
            }           
            return CoursesDTO;
        }

        /// <summary>
        /// обираємо всіх користувачів, які придбали певний курс
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task <ICollection<UserDTO>> GetUsers(int courseId)
        {
            var userCourses = _userCourseRepository.GetUserCoursesByCourseId(courseId);
            if (userCourses.Count==0)
            {
                throw new ArgumentNullException(nameof(userCourses), "This course has never been taken.");
            }
            var users = new List<UserDTO>();
            foreach (var item in userCourses)
            {
                var user = await _cRUD_UserRepository.FindCurrentUserById(item.UserId);
                if(user == null)
                {
                    throw new ArgumentNullException(nameof(userCourses), "User does not exist!");
                }
                var userDTO = new UserDTO()
                {
                    Id = item.UserId,                   
                };
                userDTO.Email=user.Email;
                userDTO.FirstName = user.FirstName;
                userDTO.LastName = user.LastName;
                users.Add(userDTO);
            }
            return users;
        }

        public Task<bool> UpdateUserCourse(int id)
        {
            throw new NotImplementedException();
        }
    }
}
