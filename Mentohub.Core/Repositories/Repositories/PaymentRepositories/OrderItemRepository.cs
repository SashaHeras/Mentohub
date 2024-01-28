using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Interfaces.PaymentInterfaces;
using Mentohub.Domain.Data.DTO.Payment;
using Mentohub.Domain.Data.Entities.CourseEntities;
using Mentohub.Domain.Data.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Repositories.Repositories.PaymentRepository
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        private readonly ProjectContext _projectContext;

        public OrderItemRepository(ProjectContext projectContext):base(projectContext)
        {
            _projectContext = projectContext;
        }
        
        public void Delete(OrderItem currentOrderItem)
        {
            _projectContext.OrderItem.Remove(currentOrderItem);
            _projectContext.SaveChanges();
        }
        
    }
}
