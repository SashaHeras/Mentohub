using Mentohub.Domain.Data.Entities.Order;

namespace Mentohub.Core.Services.Interfaces.PaymentInterfaces
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
