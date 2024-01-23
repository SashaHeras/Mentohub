using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Domain.Data.Entities.Order
{
    public class Currency
    {
        public Currency()
        {

        }
        public int ID { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public virtual ICollection<OrderPayment> OrderPayments { get; set; }
    }
}
