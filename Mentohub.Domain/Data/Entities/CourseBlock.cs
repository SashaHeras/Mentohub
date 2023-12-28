using Mentohub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Domain.Data.Entities
{
    public class CourseBlock
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int CourseID { get; set; }

        public int OrderNumber { get; set; }

        public Course Course { get; set; }

        public List<CourseItem> CourseItems { get; set; }
    }
}
