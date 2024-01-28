using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Interfaces;
using Mentohub.Core.Repositories.Interfaces.CourseInterfaces;
using Mentohub.Core.Repositories.Interfaces.PaymentInterfaces;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.Data.DTO.Payment;
using Mentohub.Domain.Data.Entities.Order;
using Mentohub.Domain.Helpers;
using System.Linq;

namespace Mentohub.Core.Services.Services.PaymentServices
{
    public class OrderService : IOrderService
    {
        private readonly ICRUD_UserRepository _cRUD_UserRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IOrderPaymentRepository _orderPaymentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICourseRepository _courseRepository;
        
        public OrderService(ICRUD_UserRepository cRUD_UserRepository,
            IOrderItemRepository orderItemRepository,
            IOrderPaymentRepository orderPaymentRepository,
            IOrderRepository orderRepository,
            ICourseRepository courseRepository)
        {
            _cRUD_UserRepository = cRUD_UserRepository;
            _orderItemRepository = orderItemRepository;
            _orderPaymentRepository = orderPaymentRepository;
            _orderRepository = orderRepository;
            _courseRepository = courseRepository;
         
        }

        public async Task<Order> CreateOrder(string userID)
        {
            var encriptId = MentoShyfr.Decrypt(userID);
            var currentUser = await _cRUD_UserRepository.FindCurrentUserById(encriptId);
            if(currentUser == null)
            {
                throw new Exception("Unknown user!");
            }

            Order order = new Order()
            {
                Created = DateTime.Now,
                UserID = currentUser.Id
            };

            _orderRepository.Add(order);
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

        public Order  GetOrder(string orderId)
        {
            var order= _orderRepository.GetOrder(orderId);               
            return order;
        }
        public OrderDTO GetOrderDTO(string orderId)
        {
            var order = _orderRepository.GetOrder(orderId);
            return new OrderDTO()
            {
                ID=order.ID,
                DiscountSum=order.DiscountSum,
                SubTotal=order.SubTotal,
                Total=order.Total,
                Created=order.Created,
                UserID=order.UserID,
               
            };
        }
        public async Task<OrderDTO> GetActiveUserOrder(string userID,int courseId)
        {
            var encriptId = MentoShyfr.Decrypt(userID);
            var currentUser = await _cRUD_UserRepository.FindCurrentUserById(encriptId);
            if (currentUser == null)
            {
                throw new Exception("Unknown user!");
            }
            var course = _courseRepository.FirstOrDefault(c => c.Id == courseId);
            if (course == null)
            {
                throw new Exception("Unknown course!");
            }
            var order = _orderRepository.GetAll(x => x.UserID == currentUser.Id && 
                                                     x.Ordered == null)
                                        .FirstOrDefault();            
            if (order== null)
            {
                var newOrder= await CreateOrder(userID);
                decimal subtotal = _courseRepository.FirstOrDefault(x => x.Id == courseId).Price;
                decimal discount = 0;
                var orderItem = new OrderItem()
                {
                    Pos= newOrder.OrderPayments.Count + 1,
                    Price = _courseRepository.FirstOrDefault(x => x.Id == courseId).Price,
                    HasDiscount = false,
                    Discount = discount,
                    OrderID = newOrder.ID,
                    CourseID = courseId,
                    SubTotal = subtotal,
                    Total= subtotal-discount,
                };

                    _orderItemRepository.Add(orderItem);
                newOrder.OrderItems.Add(orderItem);
                newOrder.DiscountSum = +orderItem.Discount;
                newOrder.SubTotal = +orderItem.SubTotal;
                newOrder.Total = (decimal)(newOrder.SubTotal - newOrder.DiscountSum);
                _orderRepository.Add(newOrder);
                return new OrderDTO()
                {
                    ID = newOrder.ID,
                    UserID = currentUser.Id,
                    Total = newOrder.Total,
                    Created = newOrder.Created,
                    Ordered = newOrder.Ordered,
                    DiscountSum = newOrder.DiscountSum,
                    SubTotal = newOrder.SubTotal,
                    Items = (List<OrderItemDTO>)newOrder.OrderItems,
                };
            }
            var currentOrderItems = order.OrderItems.Select(x => new OrderItemDTO()
            {
                ID = x.ID,
                CourseID = x.CourseID,
                Price = x.Price,
                Total = x.Total,
                SubTotal = x.SubTotal,
                Discount = x.Discount,
                HasDiscount = x.HasDiscount,
                Pos = x.Pos,
            })
            .ToList();

            return new OrderDTO()
            {
                ID = order.ID,
                UserID = currentUser.Id,
                Total = order.Total,
                Created = order.Created,
                Ordered = order.Ordered,
                DiscountSum = order.DiscountSum,
                SubTotal = order.SubTotal,
                Items = currentOrderItems
            };
        }
    }
}
