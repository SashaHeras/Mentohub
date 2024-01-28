using Mentohub.Core.Repositories.Interfaces.PaymentInterfaces;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.Data.DTO.Payment;
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

        
        public OrderItem CreateOrderItem(OrderItemDTO orderItemDTO)
        {
            return _orderItemRepository.Create(orderItemDTO);
             
        }

        public void DeleteOrderItem(int id)
        {
            var orderItem =_orderItemRepository.FirstOrDefault(x=>x.ID==id);
            if(orderItem == null) 
            {
                throw new ArgumentNullException(nameof(OrderItem), "This object does not exist");
            }

            _orderItemRepository.Delete(orderItem);
        }

        public OrderItem GetOrderItem(int id)
        {
            var orderItem = _orderItemRepository.FirstOrDefault(x=>x.ID==id);
            if (orderItem == null)
            {
                throw new ArgumentNullException(nameof(OrderItem), "This object does not exist");
            }

            return orderItem;
        }

        public List<OrderItemDTO> GetOrderItems(string id)
        {
            return _orderItemRepository.GetAll(x => x.OrderID == id)
                .Select(x => new OrderItemDTO()
                {
                    ID = x.ID,
                    OrderID = x.OrderID,
                    Price=x.Price,
                    CourseID=x.CourseID,
                    Total=x.Price,
                    SubTotal=x.SubTotal,
                    HasDiscount=x.HasDiscount,
                    Discount=x.Discount
                })
                .ToList();
        }

    }
}
