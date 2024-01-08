using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Domain.Data.Entities.CourseEntities
{
    public class CourseTag
    {
        public Guid ID { get; set; }

        public int CourseID { get; set; }

        public int TagID { get; set; }

        public virtual Course Course { get; set; }

        public virtual Tag Tag { get; set; }
    }
}
