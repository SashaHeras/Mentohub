using Mentohub.Domain.Data.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Interfaces.PaymentInterfaces
{
    public interface IOrderPaymantService
    {
        ICollection<OrderPayment> GetOrderPayments(string id);

        bool DeleteOrderPayment(string id);        
    }
}
