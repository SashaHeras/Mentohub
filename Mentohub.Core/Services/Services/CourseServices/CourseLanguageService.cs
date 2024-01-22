using Mentohub.Core.Repositories.Interfaces.CourseInterfaces;
using Mentohub.Core.Services.Interfaces.CourseInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Services.CourseServices
{
    public class CourseLanguageService : ICourseLanguageService
    {
        private readonly ICourseLanguageRepository _courseLanguageRepository;

        public CourseLanguageService(
            ICourseLanguageRepository courseLanguageRepository
            )
        {
            _courseLanguageRepository = courseLanguageRepository;
        }

        public List<KeyValuePair<int, string>> GetLanguagesList()
        {
            var res = _courseLanguageRepository.GetAll()
                                               .ToList()
                                               .Select(x => new KeyValuePair<int, string>(x.Id, x.Name))
                                               .ToList();

            return res.OrderBy(x => x.Key).ToList();
        }
    }
}
