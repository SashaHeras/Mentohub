using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.DTO.Payment;
using Mentohub.Domain.Data.Entities;
using Mentohub.Domain.Data.Entities.CourseEntities;
using Mentohub.Domain.Data.Entities.Order;

namespace Mentohub.Core.Services.Interfaces
{
    public interface IUserCourseService
    {
        UserCourse CreateUserCourse(UserCourseDTO data);

        bool DeleteUserCourse(int id);

        UserCourse GetUserCourse(int id);

        ICollection<Course> GetUserCourses(string userId);

        ICollection<UserDTO> GetUsers(int courseId);
    }
}
