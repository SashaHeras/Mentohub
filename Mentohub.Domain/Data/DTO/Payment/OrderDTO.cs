using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Domain.Data.DTO.Payment
{
    public class OrderDTO
    {
        public string ID { get; set; }

        public decimal Total { get; set; }

        public Nullable<DateTime> Created { get; set; }

        public Nullable<DateTime> Ordered { get; set; }

        public Nullable<decimal> DiscountSum { get; set; }

        public Nullable<decimal> SubTotal { get; set; }

        public string UserID { get; set; }

        public List<OrderItemDTO> Items { get; set; }
    }
}
