using Mentohub.Core.Repositories.Interfaces;
using Mentohub.Core.Repositories.Interfaces.CourseInterfaces;
using Mentohub.Core.Repositories.Interfaces.PaymentInterfaces;
using Mentohub.Core.Services.Interfaces.PaymentInterfaces;
using Mentohub.Domain.Data.DTO.Payment;
using Mentohub.Domain.Data.Entities.Order;


namespace Mentohub.Core.Services.Services.PaymentServices
{
    public class OrderPaymantService : IOrderPaymantService
    {
        private readonly IOrderPaymentRepository _orderPaymentRepository;
        private readonly IUserCourseRepository _userCourseRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IOrderRepository _orderRepository;

        public OrderPaymantService(
            IOrderPaymentRepository orderPaymentRepository,
            IUserCourseRepository userCourseRepository,
            IOrderItemRepository orderItemRepository,
            IOrderRepository orderRepository
        )
        {
            _orderPaymentRepository = orderPaymentRepository;
            _userCourseRepository = userCourseRepository;
            _orderItemRepository = orderItemRepository;
            _orderRepository = orderRepository;
        }

        public async Task<OrderPayment> CreateOrderPaymant(decimal total, int currencyID, string orderID)
        {
            var order = _orderRepository.FirstOrDefault(o => o.ID == orderID);
            if(order == null)
            {
                throw new Exception("Order does not exist");
            }

            var orderItem = _orderItemRepository.GetAll().Where(o => o.OrderID == orderID).ToList();
            if (orderItem.Count == 0)
            {
                throw new Exception("OrderItem does not exist");
            }

            var userCourses = new List<UserCourse>();
            var orderPayment = new OrderPayment()
            {
                ID = Guid.NewGuid().ToString(),
                Created = DateTime.Now,
                CurrencyID = currencyID,
                OrderID = orderID,
                UserCourses = new List<UserCourse>(),
                Total = total,
                PaymentStatus = 1,
            };

            orderPayment = _orderPaymentRepository.AddOrderPayment(orderPayment);

            var currentUserCoursesNumber = _userCourseRepository.GetAll().Count();
            foreach (var item in orderItem)
            {               
                var userCourse = new UserCourse()
                {
                    Id = ++currentUserCoursesNumber,
                    Created = DateTime.Now,
                    CourseId = (int)item.CourseID,
                    OrderItemId = (int)item.ID,
                    UserId = order.UserID,
                    OrderPaymentId = orderPayment.ID
                };

                userCourses.Add(userCourse);
            }

            _userCourseRepository.AddList(userCourses);
            var result = _orderPaymentRepository.FirstOrDefault(x => x.ID == orderPayment.ID);
            
            return result;
        }

        public bool DeleteOrderPayment(string id)
        {
            var order = _orderPaymentRepository.FirstOrDefault(x => x.ID == id);
            if(order == null)
            {
                throw new ArgumentNullException(nameof(order), "The Order does not exist");
            }

            _orderPaymentRepository.DeleteOrderPayment(order);
            return true;
        }

        public ICollection<OrderPaymentDTO> GetOrderPayments(string id)
        {
            var orders = _orderPaymentRepository.GetAll(x => x.ID == id);
            if(orders == null)
            {
                throw new ArgumentNullException(nameof(orders), "The Orders does not exist");
            }

            var ordersDTO = new List<OrderPaymentDTO>();
            foreach(var order in orders)
            {
                ordersDTO.Add(new OrderPaymentDTO()
                {
                    ID = order.ID,
                    Created = order.Created,
                    PaymentStatus = order.PaymentStatus,
                    OrderID = order.OrderID,
                    CurrencyID = order.CurrencyID,
                    Total = order.Total,
                });
            }

            return ordersDTO;
        }
    }
}
