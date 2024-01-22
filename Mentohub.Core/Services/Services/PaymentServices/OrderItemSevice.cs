using Mentohub.Core.Repositories.Interfaces.PaymentInterfaces;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.Data.Entities.Order;

namespace Mentohub.Core.Services.Services.PaymentServices
{
    public class OrderItemSevice : IOrderItemSevice
    {
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderItemSevice(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        public OrderItem CreateOrderItem(decimal price, decimal total, decimal? discount, bool? hasDiscount, string orderID, int courseID)
        {
            if(orderID == null)
            {
                throw new ArgumentException("Operation cannot be performed");
            }

            var orderItem = new OrderItem();// _orderItemRepository.AddOrderItem(price, total, discount, hasDiscount, orderID, courseID);
            return orderItem;
        }

        public void DeleteOrderItem(int id)
        {
            var orderItem=_orderItemRepository.GetOrderItem(id);
            if(orderItem == null) 
            {
                throw new ArgumentNullException(nameof(OrderItem), "This object does not exist");
            }
            //_orderItemRepository.Re(orderItem);
        }

        public OrderItem GetOrderItem(int id)
        {
            var orderItem = _orderItemRepository.GetOrderItem(id);
            if (orderItem == null)
            {
                throw new ArgumentNullException(nameof(OrderItem), "This object does not exist");
            }
            return orderItem;
        }

        public ICollection<OrderItem> GetOrders()
        {
            var orderItems = _orderItemRepository.GetOrderItems();
            if(orderItems == null)
            {
                throw new ArgumentNullException(nameof(ICollection<OrderItem>), "This object does not exist");
            }

            return orderItems;
        }

        public void UpdateOrderItem(int id)
        {
            throw new NotImplementedException();
        }
    }
}
