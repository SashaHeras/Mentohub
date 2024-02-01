using Mentohub.Domain.Data.Entities.CourseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Domain.Data.Entities.Order
{
    public class OrderItem
    {
        [Key]
        public int? ID { get; set; }
        
        public int? Pos { get; set; }

        public decimal? Price { get; set; }

        public decimal? Total { get; set; }

        public Nullable<decimal> SubTotal { get; set; }
       
        public Nullable<decimal> Discount { get; set; }

        public Nullable<bool> HasDiscount { get; set; }

        public string? OrderID { get; set; }

        public int CourseID { get; set; }

        public virtual Order Order { get; set; }

        public virtual Course Course { get; set; }

        public virtual UserCourse? UserCourse { get; set; }
    }
}
