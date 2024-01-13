using Mentohub.Domain.Data.DTO.CourseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Domain.Data.DTO.ResultDTO
{
    public class SearchCourseResult
    {
        public int TotalCount { get; set; }

        public List<CourseDTO> Courses { get; set; }
    }
}
