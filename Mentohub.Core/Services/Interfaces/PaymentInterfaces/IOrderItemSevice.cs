using Mentohub.Domain.Data.DTO.Payment;
using Mentohub.Domain.Data.Entities.Order;

namespace Mentohub.Core.Services.Interfaces
{
    public interface IOrderItemSevice
    {
        OrderItem GetOrderItem(int id);
        OrderItem CreateOrderItem(OrderItemDTO orderItemDTO);
        void DeleteOrderItem(int id);
        List<OrderItemDTO> GetOrderItems(string id);
    }
}
