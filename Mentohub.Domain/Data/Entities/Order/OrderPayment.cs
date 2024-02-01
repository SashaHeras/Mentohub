using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Domain.Data.Entities.Order
{
    public class OrderPayment
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }

        public DateTime Created { get; set; }

        public decimal Total { get; set; }

        public int PaymentStatus { get; set; }

        public int CurrencyID { get; set; }
       
        public string OrderID { get; set; }

        public virtual Order Order { get; set; }

        public virtual Currency Currency { get; set; }

        public virtual ICollection<UserCourse> UserCourses { get; set; }
    }
}
