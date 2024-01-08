using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Domain.Data.Entities.CourseEntities
{
    public class CourseBlock
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int CourseID { get; set; }

        public int OrderNumber { get; set; }

        public virtual Course Course { get; set; }

        public virtual List<CourseItem> CourseItems { get; set; }
    }
}
