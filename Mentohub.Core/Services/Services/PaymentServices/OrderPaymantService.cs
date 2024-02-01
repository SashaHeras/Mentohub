
using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Interfaces.PaymentInterfaces;
using Mentohub.Core.Repositories.Repositories.PaymentRepository;
using Mentohub.Core.Services.Interfaces.PaymentInterfaces;
using Mentohub.Domain.Data.Entities.CourseEntities;
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

        public OrderPayment CreateOrderPaymant(decimal total, int currencyID, string orderID)
        {
            var orderItem = _orderItemRepository.GetAll().Where(o => o.OrderID == orderID).ToList();
            var order = _orderRepository.FirstOrDefault(o => o.ID == orderID);
            if (orderItem.Count==0 )
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
                UserCourses = userCourses,
                Total = total,
                PaymentStatus = 1,                           
            };
            _orderPaymentRepository.AddOrderPayment(orderPayment);
            foreach (var item in orderItem)
            {
                var userCourse = new UserCourse()
                {
                    Id = _userCourseRepository.GetAll().Count() + 1,
                    Created = DateTime.Now,
                    CourseId = item.CourseID,
                    OrderItemId = item.ID,
                    UserId = order.UserID,
                    OrderPaymentId = orderPayment.ID,
                };
                userCourses.Add(userCourse);
                _userCourseRepository.Add(userCourse); 
            }
            orderPayment.UserCourses = userCourses;
            var result =_orderPaymentRepository.Update(orderPayment);           
            return result;
        }

        public bool DeleteOrderPayment(string id)
        {
            var order = _orderPaymentRepository.FirstOrDefault(x=>x.ID == id);
            if(order == null)
            {
                throw new ArgumentNullException(nameof(order), "The Order does not exist");
            }

            _orderPaymentRepository.DeleteOrderPayment(order);
            return true;
        }

        public ICollection<OrderPayment> GetOrderPayments(string id)
        {
            var orders = _orderPaymentRepository.GetAll(x => x.OrderID == id).ToList();
            if(orders == null)
            {
                throw new ArgumentNullException(nameof(orders), "The Orders does not exist");
            }

            return orders;
        } 
    }
}
