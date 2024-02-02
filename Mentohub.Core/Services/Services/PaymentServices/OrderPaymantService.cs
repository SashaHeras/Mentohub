
using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Interfaces;
using Mentohub.Core.Repositories.Interfaces.CourseInterfaces;
using Mentohub.Core.Repositories.Interfaces.PaymentInterfaces;
using Mentohub.Core.Repositories.Repositories.PaymentRepository;
using Mentohub.Core.Services.Interfaces.PaymentInterfaces;
using Mentohub.Domain.Data.DTO.Payment;
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
        private readonly ICourseRepository _courseRepository;
        private readonly ICRUD_UserRepository _CRUD_Repository;
        public OrderPaymantService(
            IOrderPaymentRepository orderPaymentRepository,
            IUserCourseRepository userCourseRepository,
            IOrderItemRepository orderItemRepository,
            IOrderRepository orderRepository,
            ICourseRepository courseRepository,
            ICRUD_UserRepository CRUD_Repository
        )
        {
            _orderPaymentRepository = orderPaymentRepository;
            _userCourseRepository = userCourseRepository;
            _orderItemRepository = orderItemRepository;
            _orderRepository = orderRepository;
            _courseRepository = courseRepository;
            _CRUD_Repository=CRUD_Repository;
        }

        public async Task<OrderPayment> CreateOrderPaymant(decimal total, int currencyID, string orderID)
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
            
            //var user = await _CRUD_Repository.FindCurrentUserById(order.UserID);
            //if(user==null)
            //{
            //    throw new ArgumentNullException("User does not exist!");
            //}
            foreach (var item in orderItem)
            {
                //var course = _courseRepository.GetCourse(item.CourseID);
                //if (course == null)
                //{
                //    throw new Exception($"Course with ID {item.CourseID} not found");
                //}
                var userCourse = new UserCourse()
                {
                    Id = _userCourseRepository.GetAll().Count() + 1,
                    Created = DateTime.Now,
                    CourseId = item.CourseID,
                    OrderItemId = item.ID,
                    UserId = order.UserID,
                    OrderPaymentId = orderPayment.ID,
                    //OrderPayment=orderPayment,
                    //Course=course,
                    OrderItem=item,
                    //СurrentUser=user,
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

        public ICollection<OrderPaymentDTO> GetOrderPayments(string id)
        {
            var orders = _orderPaymentRepository.GetAll(x => x.ID == id);
            if(orders == null)
            {
                throw new ArgumentNullException(nameof(orders), "The Orders does not exist");
            }
            var ordersDTO=new List<OrderPaymentDTO>();
            foreach(var order in orders)
            {
                ordersDTO.Add(new OrderPaymentDTO()
                {
                     ID=order.ID,
                     Created=order.Created,
                     PaymentStatus=order.PaymentStatus,
                     OrderID=order.OrderID,
                     CurrencyID=order.CurrencyID,
                     Total=order.Total,                    
                });
            }
            return ordersDTO;
        } 
    }
}
