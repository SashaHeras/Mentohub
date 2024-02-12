using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Domain.Data.DTO.CourseDTOs
{
    public class CourseElementDTO
    {
        public int CourseItemId { get; set; }

        public string ItemID { get; set; }

        public int TypeId { get; set; }

        public int CourseId { get; set; }

        public DateTime DateCreation { get; set; }

        public int OrderNumber { get; set; }

        public string ElementName { get; set; }
    }
}
