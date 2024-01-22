using Mentohub.Core.Repositories.Interfaces.CourseInterfaces;
using Mentohub.Core.Services.Interfaces.CourseInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Services.CourseServices
{
    public class CourseLevelService : ICourseLevelService
    {
        private readonly ICourseLevelRepository _courseLevelRepository;

        public CourseLevelService(
            ICourseLevelRepository courseLevelRepository
        )
        {
            _courseLevelRepository = courseLevelRepository;
        }

        public List<KeyValuePair<int, string>> GetLevelsList()
        {
            var res = _courseLevelRepository.GetAll()
                                            .Select(x => new KeyValuePair<int, string>(x.ID, x.Name))
                                            .ToList();

            return res.OrderBy(x => x.Key).ToList();
        }
    }
}
