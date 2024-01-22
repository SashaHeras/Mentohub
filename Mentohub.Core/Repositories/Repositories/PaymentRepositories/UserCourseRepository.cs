using MassTransit;
using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Interfaces.PaymentInterfaces;
using Mentohub.Domain.Data.Entities.CourseEntities;
using Mentohub.Domain.Data.Entities.Order;
using Mentohub.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Repositories.Repositories.PaymentRepository
{
    public class UserCourseRepository : Repository<UserCourse>, IUserCourseRepository
    {
        #pragma warning disable 8603
        private readonly ProjectContext _projectContext;
        
        public UserCourseRepository(ProjectContext projectContext) : base(projectContext)
        {
            _projectContext = projectContext;
        }

        public UserCourse GetUserCourseById(int Id)
        {
            return GetAll().Where(u => u.Id == Id).FirstOrDefault();
        }

        public ICollection<UserCourse> GetUserCoursesByUserId(string userId)
        {
            var decriptUserId = MentoShyfr.Decrypt(userId);
            return GetAll().Where(uc => uc.UserId == decriptUserId).ToList();
        }
        public ICollection<UserCourse> GetUserCoursesByCourseId(int courseId)
        {
            return GetAll().Where(uc => uc.CourseId == courseId).ToList();
        }

        public  UserCourse AddUserCourse(int courseId,string userId)
        {
            var decriptUserId = MentoShyfr.Decrypt(userId);
            var userCourses = _projectContext.UserCourses.ToList();
            var userCourse = new UserCourse(courseId, decriptUserId);
            if (userCourses.Count == 0) 
            {
                userCourse.Id = 1;
            }
            var lastUserCourseId=_projectContext.UserCourses.Max(u => u.CourseId);
            userCourse.Id = lastUserCourseId + 1;
            _projectContext.UserCourses.Add(userCourse);
            _projectContext.SaveChanges();
            return userCourse;
        }

        public void UpdateUserCourse(UserCourse userCourse)
        {
            _projectContext.Update(userCourse);
            _projectContext.SaveChanges();            
        }

        public void DeleteUserCourse(UserCourse userCourse)
        {
            _projectContext.Remove(userCourse);
            _projectContext.SaveChanges();
        }
    }
}
