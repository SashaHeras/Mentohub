using Mentohub.Domain.Data.Entities.Order;

namespace Mentohub.Core.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreatOrder(decimal total, string userID, decimal discountSum);
        Order UpDateOrder(string orderId, decimal discount, decimal total);
        bool DeleteOrder(string orderId);
        Order GetOrder(string orderId);
        IQueryable<Order> GetOrders();
    }
}
