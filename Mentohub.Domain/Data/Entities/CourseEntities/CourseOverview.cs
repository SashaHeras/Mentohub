using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Domain.Data.Entities.CourseEntities
{
    public class CourseOverview
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int CourseID { get; set; }

        public virtual Course Course { get; set; }
    }
}
