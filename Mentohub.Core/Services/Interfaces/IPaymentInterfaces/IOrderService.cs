using Mentohub.Domain.Data.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Interfaces.IPaymentInterfaces
{
    public interface IOrderService
    {
        Task<Order> CreatOrder (decimal total, string userID, decimal discountSum);
        Order UpDateOrder(string orderId, decimal discount, decimal total);
        bool DeleteOrder(string orderId);
        Order GetOrder(string orderId);
        ICollection<Order> GetOrders();
    }
}
