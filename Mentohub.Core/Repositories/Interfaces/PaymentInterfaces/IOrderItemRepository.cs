using Mentohub.Core.Infrastructure;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Domain.Data.DTO.Payment;
using Mentohub.Domain.Data.Entities.Order;

namespace Mentohub.Core.Repositories.Interfaces.PaymentInterfaces
{
    public interface IOrderItemRepository : ISingletoneService, IRepository<OrderItem>
    {
        public void Delete(OrderItem currentOrderItem);
        public OrderItem Create();
        void UpDate(OrderItemDTO OrderItemDTO, OrderItem orderItem);
    }
}
