using Mentohub.Domain.Data.DTO.Payment;
using Mentohub.Domain.Data.Entities.Order;

namespace Mentohub.Core.Services.Interfaces
{
    public interface IOrderService : IService
    {
        Task<Order> CreateOrder(string userID);

        bool DeleteOrder(string orderId);

        Order GetOrder(string orderId);

        Task<OrderDTO> GetActiveUserOrder(string userID, int courseId);

        OrderDTO GetOrderDTO(string orderId);

        Order UpdateOrder(Order order);
    }
}
