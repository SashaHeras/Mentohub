using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Domain.Helpers
{
    public class CreateOrderPayment
    {
        public int CurrencyId { get; set; }
        public string? OrderId { get; set; }  
        public decimal Total  { get; set; }
    }
}
