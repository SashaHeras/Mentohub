using Mentohub.Core.Infrastructure;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Domain.Data.Entities.Order;

namespace Mentohub.Core.Repositories.Interfaces.PaymentInterfaces
{
    public interface IOrderItemRepository : ISingletoneService, IRepository<OrderItem>
    {
        public ICollection<OrderItem> GetOrderItems();

        public OrderItem GetOrderItem(int id);

        public void Delete(OrderItem currentOrderItem);
    }
}
