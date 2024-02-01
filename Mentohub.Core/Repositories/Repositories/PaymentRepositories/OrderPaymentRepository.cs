using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Core.Repositories.Interfaces.PaymentInterfaces;
using Mentohub.Domain.Data.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Repositories.Repositories.PaymentRepository
{
    public class OrderPaymentRepository : Repository<OrderPayment>, IOrderPaymentRepository
    {
        private readonly ProjectContext _projectContext;
        #pragma warning disable 8603
        public OrderPaymentRepository(ProjectContext projectContext) : base(projectContext)
        {
            _projectContext = projectContext;
        }
        
        public OrderPayment AddOrderPayment(OrderPayment orderPayment)
        {         
            _projectContext.Add(orderPayment);
            _projectContext.SaveChanges();
            return orderPayment;
        }

        public void DeleteOrderPayment(OrderPayment orderPayment)
        { 
            _projectContext.Remove(orderPayment);
            _projectContext.SaveChanges();
        }

        public OrderPayment GetOrderPaymentById(string id)
        {
           return GetAll(op => op.ID == id).FirstOrDefault();            
        }

        public void UpdateOrderPayment(OrderPayment orderPayment)
        {
            _projectContext.Update(orderPayment);
            _projectContext.SaveChanges();
        }
    }
}
