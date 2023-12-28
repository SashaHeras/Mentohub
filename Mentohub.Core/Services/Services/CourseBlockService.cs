using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Core.Repositories.Interfaces;
using Mentohub.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Services
{
    public class CourseBlockService : ICourseBlockService
    {
        private readonly ICourseBlockRepository _courseBlockRepository;

        public CourseBlockService(
            ICourseBlockRepository courseBlockRepository
        )
        {
            _courseBlockRepository = courseBlockRepository;
        }
    }
}
