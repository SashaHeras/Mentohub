using Mentohub.Domain.Data.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Interfaces.IPaymentInterfaces
{
    public interface IOrderItemSevice
    {
        OrderItem GetOrderItem(int id);
        OrderItem CreateOrderItem(decimal price, decimal total, decimal? discount,
            bool? hasDiscount, string orderID, int courseID);
        ICollection<OrderItem> GetOrders();
        void DeleteOrderItem(int id);
        void UpdateOrderItem(int id);

    }
}
