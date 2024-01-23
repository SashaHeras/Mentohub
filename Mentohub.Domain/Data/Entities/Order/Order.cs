using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Domain.Data.Entities.Order
{
    public class Order
    {
        
        public string ID { get; set; }

        public decimal Total { get; set; }

        public Nullable<DateTime> Created { get; set; }

        public Nullable<DateTime> Ordered { get; set; }
        
        public Nullable<decimal> DiscountSum { get; set; }
        
        public Nullable<decimal> SubTotal { get; set; }
        
        public string UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual CurrentUser User { get; set; } 

        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public virtual ICollection<OrderPayment> OrderPayments { get; set; }
    }
}
