using Mentohub.Core.Repositories.Interfaces.CourseInterfaces;
using Mentohub.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Services
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

            res.Add(new KeyValuePair<int, string>(-1, "Будь-яка"));

            return res.OrderBy(x => x.Key).ToList();
        }
    }
}
