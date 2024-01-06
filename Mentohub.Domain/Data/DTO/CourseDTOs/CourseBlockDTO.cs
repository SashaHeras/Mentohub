using Mentohub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Domain.Data.DTO.CourseDTOs
{
    public class CourseBlockDTO
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int CourseID { get; set; }

        public int OrderNumber { get; set; }

        public int LessonsCount { get; set; } = 0;

        public int TestCount { get; set; } = 0;

        public string TotalTime { get; set; }

        public List<CourseElementDTO> CourseItems { get; set; }
    }
}
