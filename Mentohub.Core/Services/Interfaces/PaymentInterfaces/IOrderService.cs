using Mentohub.Domain.Data.Entities.Order;

namespace Mentohub.Core.Services.Interfaces.PaymentInterfaces
{
    public interface IOrderService
    {
        Task<Order> CreatOrder(decimal total, string userID, decimal discountSum);
        Order UpDateOrder(string orderId, decimal discount, decimal total);
        bool DeleteOrder(string orderId);
        Order GetOrder(string orderId);
        ICollection<Order> GetOrders();
    }
}
