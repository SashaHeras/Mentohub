using Mentohub.Domain.Data.Entities;
using Mentohub.Domain.Data.Entities.CourseEntities;
using Mentohub.Domain.Data.Entities.Order;

namespace Mentohub.Core.Services.Interfaces
{
    public interface IUserCourseService
    {
        IUserCourseService CreateUserCourse(int courseId, string userId);
        Task<bool> UpdateUserCourse(int id);
        bool DeleteUserCourse(int id);
        UserCourse GetUserCourse(int id);
        ICollection<Course> GetUserCourses(string userId);
        ICollection<CurrentUser> GetUsers(int courseId);

    }
}
