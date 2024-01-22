using Mentohub.Core.Repositories.Interfaces.PaymentInterfaces;
using Mentohub.Core.Services.Interfaces.IPaymentInterfaces;
using Mentohub.Domain.Data.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if(orderID==null )
            {
                throw new ArgumentException("Operation cannot be performed");
            }
            var orderItem = _orderItemRepository.AddOrderItem(price, total, discount, hasDiscount, orderID, courseID);
            return orderItem;
        }

        public void DeleteOrderItem(int id)
        {
            var orderItem=_orderItemRepository.GetOrderItem(id);
            if(orderItem == null) 
            {
                throw new ArgumentNullException(nameof(OrderItem), "This object does not exist");
            }
            _orderItemRepository.DeleteOrderItem(orderItem);
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
            var orderItems = _orderItemRepository.GetAllOrderItems();
            if(orderItems==null)
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
