using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Interfaces;
using Mentohub.Core.Repositories.Interfaces.PaymentInterfaces;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.Data.Entities.Order;
using Mentohub.Domain.Helpers;

namespace Mentohub.Core.Services.Services.PaymentServices
{
    public class OrderService : IOrderService
    {
        private readonly ICRUD_UserRepository _cRUD_UserRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IOrderPaymentRepository _orderPaymentRepository;
        private readonly IOrderRepository _orderRepository;
        public OrderService(ICRUD_UserRepository cRUD_UserRepository,
            IOrderItemRepository orderItemRepository,
            IOrderPaymentRepository orderPaymentRepository,
            IOrderRepository orderRepository)
        {
            _cRUD_UserRepository = cRUD_UserRepository;
            _orderItemRepository = orderItemRepository;
            _orderPaymentRepository = orderPaymentRepository;
            _orderRepository = orderRepository;
        }

        public async Task<Order> CreatOrder(decimal total, string userID, decimal discountSum)
        {
            var encriptId=MentoShyfr.Decrypt(userID);
            Order order = new Order(total, encriptId, discountSum);
            order.User = await _cRUD_UserRepository.FindCurrentUserById(encriptId);
            order.OrderItems = _orderItemRepository.GetOrderItems();
            order.OrderPayments = _orderPaymentRepository.GetAll().ToList();
            order.Ordered = DateTime.Now;
            _orderRepository.AddOrder(order);
            return order;
        }

        public bool DeleteOrder(string orderId)
        {
            var order = _orderRepository.GetOrder(orderId);
            if (order == null)
            {
                return false;
                throw new ArgumentNullException(nameof(order), "The Order does not exist"); 
            }

            _orderRepository.DeleteOrder(order);
            return true;
        }

        public Order GetOrder(string orderId)
        {
            var order= _orderRepository.GetOrder(orderId);
            if (order == null)
                throw new ArgumentNullException(nameof(order), "The Order does not exist");
            return order;
        }

        public IQueryable<Order> GetOrders()
        {
            var orders = _orderRepository.GetAll();
            if (orders == null)
            {
                throw new ArgumentNullException(nameof(orders), "Collection does not exist");
            }

            return orders;           
        }

        public Order UpDateOrder(string orderId,decimal discount,decimal total)
        {
            var order = GetOrder(orderId);
            if (order == null)
                throw new ArgumentNullException(nameof(order),"The Order does not exist");
            order.DiscountSum = discount;
            order.Total = total;
            _orderRepository.UpdateOrder(order);
            return order;
        }
    }
}
