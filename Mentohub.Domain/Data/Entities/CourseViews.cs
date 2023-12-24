using Mentohub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Domain.Data.Entities
{
    public class CourseViews
    {
        [Key]
        public Guid ID { get; set; }

        public DateTime ViewDate { get; set; }

        public string UserID { get; set; }

        public int CourseID { get; set; }

        [ForeignKey("UserID")]
        public CurrentUser User { get; set; }

        [ForeignKey("CourseID")]
        public Course Course { get; set; }
    }
}
