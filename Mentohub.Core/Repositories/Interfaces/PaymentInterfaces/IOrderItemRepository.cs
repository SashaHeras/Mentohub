using Mentohub.Domain.Data.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Repositories.Interfaces.PaymentInterfaces
{
    public interface IOrderItemRepository
    {
        ICollection<OrderItem> GetAllOrderItems();
        OrderItem GetOrderItem(int id);
        OrderItem AddOrderItem(decimal price, decimal total, decimal? discount,
            bool? hasDiscount, string orderID, int courseID);
        void UpdateOrderItem(OrderItem orderItem);
        void DeleteOrderItem(OrderItem orderItem);

    }
}
