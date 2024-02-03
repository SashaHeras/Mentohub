using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Interfaces;
using Mentohub.Core.Repositories.Interfaces.CourseInterfaces;
using Mentohub.Core.Repositories.Interfaces.PaymentInterfaces;
using Mentohub.Core.Repositories.Repositories.PaymentRepository;
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
        private readonly IUserCourseRepository _userCourseRepository;

        public OrderService(ICRUD_UserRepository cRUD_UserRepository,
            IOrderItemRepository orderItemRepository,
            IOrderPaymentRepository orderPaymentRepository,
            IOrderRepository orderRepository,
            ICourseRepository courseRepository,
            IUserCourseRepository userCourseRepository)
        {
            _cRUD_UserRepository = cRUD_UserRepository;
            _orderItemRepository = orderItemRepository;
            _orderPaymentRepository = orderPaymentRepository;
            _orderRepository = orderRepository;
            _courseRepository = courseRepository;
            _userCourseRepository = userCourseRepository;
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
                UserID = currentUser.Id,
                SubTotal = 0,
                Total = 0,
                DiscountSum = 0,
                OrderItems = new List<OrderItem>()
            };

            _orderRepository.Add(order);
            return order;
        }

        public bool DeleteOrder(string orderId)
        {
            var order = _orderRepository.GetOrder(orderId);
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order), "The Order does not exist"); 
            }

            _orderRepository.DeleteOrder(order);
            return true;
        }

        public Order  GetOrder(string orderId)
        {
            var order = _orderRepository.GetOrder(orderId);
            if (order == null)
            {
                throw new Exception("Order does not exist!");
            }

            return order;
        }

        public OrderDTO GetOrderDTO(string orderId)
        {
            var order = GetOrderDTO(orderId);
            return new OrderDTO()
            {
                ID = order.ID,
                DiscountSum = order.DiscountSum,
                SubTotal = order.SubTotal,
                Total = order.Total,
                Created = order.Created,
                UserID = order.UserID,                
            };
        }

        public async Task<OrderDTO> GetActiveUserOrder(string userID, int courseId)
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

            if (order == null)
            {
                order = await CreateOrder(userID);
            }
            else
            {
                if(order.OrderItems.Any(x => x.CourseID == courseId))
                {
                    throw new Exception("Course is already in your basket!");
                }
            }
            
            decimal subtotal = course.Price;
            decimal discount = 0;
            
            var orderItem = new OrderItem()
            {
                Pos = order.OrderItems.Count + 1,
                Price = course.Price,
                HasDiscount = false,
                Discount = discount,
                OrderID = order.ID,
                CourseID = courseId,
                SubTotal = subtotal,
                Total = subtotal - discount,               
            };
            
            order.OrderItems.Add(orderItem);
            order.DiscountSum += orderItem.Discount;
            order.SubTotal += orderItem.SubTotal;
            order.Total = (decimal)(order.SubTotal - order.DiscountSum);

            _orderRepository.Update(order);

            var currentOrderItems = order.OrderItems.Select(x => new OrderItemDTO()
            {
                ID = x.ID,
                CourseID = x.CourseID,
                Price = x.Price,
                Total = x.Total,
                SubTotal = x.SubTotal,
                Discount = x.Discount,
                HasDiscount = x.HasDiscount,
                OrderID = x.OrderID,
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

        public Order UpdateOrder(Order order)
        {
            return _orderRepository.Update(order);
        }
    }
}
