using Mentohub.Domain.Data.DTO.CourseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Interfaces
{
    public interface IOverviewService : IService
    {
        public CourseOverviewDTO Apply(CourseOverviewDTO data);

        public List<CourseOverviewDTO> GetCourseOverviews(int ID);
    }
}
