using Mentohub.Domain.Data.Entities.Order;

namespace Mentohub.Core.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreatOrder(string userID);
        bool DeleteOrder(string orderId);
        Order GetOrder(string orderId);
    }
}
