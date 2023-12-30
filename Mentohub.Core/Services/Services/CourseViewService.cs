using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Core.Repositories.Interfaces;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.Data.DTO.Helpers;
using Mentohub.Domain.Data.Entities.CourseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Services
{
    public class CourseViewService : ICourseViewService
    {
        private readonly ICourseViewsRepository _courseViewsRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ICRUD_UserRepository _userRepository;

        public CourseViewService(
            ICourseViewsRepository courseViewsRepository,
            ICourseRepository courseRepository,
            ICRUD_UserRepository userRepository
            )
        {
            _courseViewsRepository = courseViewsRepository;
            _courseRepository = courseRepository;
            _userRepository = userRepository;
        }

        public async Task<CourseViews> TryAddUserView(int CourseID, string UserID)
        {
            var userID = MentoShyfr.Decrypt(UserID);
            var user =  await _userRepository.FindCurrentUserById(userID);
            if(user == null) {
                throw new Exception("User not found!");
            }

            var startDate = Helper.GetStartDateTime(DateTime.Now);
            var userViewsThisDay = _courseViewsRepository.GetAll(x => 
                                                                    x.UserID == userID && 
                                                                    x.CourseID == CourseID &&
                                                                    x.ViewDate >= startDate)
                                                         .ToList();

            if(userViewsThisDay.Count > 0)
            {
                return null;
            }

            return new CourseViews()
            {
                UserID = userID,
                CourseID = CourseID,
                ViewDate = DateTime.Now,
            };
        }

        public IQueryable<CourseViews> ViewsByCourse(int CourseID)
        {
            throw new NotImplementedException();
        }
    }
}
