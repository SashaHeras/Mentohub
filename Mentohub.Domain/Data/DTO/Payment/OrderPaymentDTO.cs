using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Domain.Data.DTO.Payment
{
    public class OrderPaymentDTO
    {
        public string ID { get; set; }

        public DateTime Created { get; set; }

        public decimal Total { get; set; }

        public int PaymentStatus { get; set; }

        public int CurrencyID { get; set; }

        public string OrderID { get; set; }
    }
}
