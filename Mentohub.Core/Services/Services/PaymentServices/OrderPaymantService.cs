
using Mentohub.Core.Repositories.Interfaces.PaymentInterfaces;
using Mentohub.Core.Services.Interfaces.PaymentInterfaces;
using Mentohub.Domain.Data.Entities.Order;


namespace Mentohub.Core.Services.Services.PaymentServices
{
    public class OrderPaymantService : IOrderPaymantService
    {
        private readonly IOrderPaymentRepository _orderPaymentRepository;
        private readonly IUserCourseRepository _userCourseRepository;

        public OrderPaymantService(
            IOrderPaymentRepository orderPaymentRepository,
            IUserCourseRepository userCourseRepository
        )
        {
            _orderPaymentRepository = orderPaymentRepository;
            _userCourseRepository = userCourseRepository;
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
