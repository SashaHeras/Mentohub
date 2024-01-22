using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Interfaces.PaymentInterfaces;
using Mentohub.Domain.Data.Entities.CourseEntities;
using Mentohub.Domain.Data.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Repositories.Repositories.PaymentRepository
{
    public class OrderItemRepository : Repository<OrderItem> ,IOrderItemRepository
    {
        private readonly ProjectContext _projectContext;

        public OrderItemRepository(ProjectContext projectContext):base(projectContext)
        {
            _projectContext = projectContext;
        }

        public OrderItem AddOrderItem(decimal price, decimal total, decimal? discount,
            bool? hasDiscount, string orderID, int courseID)
        {
            OrderItem orderItem = new OrderItem(price, total, discount,
            hasDiscount, orderID, courseID);
            var orderItems = _projectContext.OrderItem.ToList();
            if (orderItems.Count == 0)
            {
                orderItem.ID = 1;
            }
            var lastOrderItemId = _projectContext.OrderItem.Max(u => u.ID);
            orderItem.ID = lastOrderItemId + 1;
            _projectContext.OrderItem.Add(orderItem);
            _projectContext.SaveChanges();
            return orderItem;
        }

        public void DeleteOrderItem(OrderItem orderItem)
        {
            _projectContext.OrderItem.Remove(orderItem);
            _projectContext.SaveChanges();
            
        }

        public ICollection<OrderItem> GetAllOrderItems()
        {
            return GetAll().ToList();
        }

        public OrderItem GetOrderItem(int id)
        {
             return GetAll().Where(o=>o.ID==id).FirstOrDefault();
        }

        public void UpdateOrderItem(OrderItem orderItem)
        {
            throw new NotImplementedException();
        }
    }
}
