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
        
        public OrderPayment AddOrderPayment(decimal total, int currencyID, string orderID)
        {
            OrderPayment orderPayment = new OrderPayment();
            var userCourses= _projectContext.UserCourses.ToList();
            orderPayment.UserCourses = userCourses;
            _projectContext.OrderPayment.Add(orderPayment);
            _projectContext.SaveChanges();
            return orderPayment;
        }

        public void DeleteOrderPayment(OrderPayment orderPayment)
        { 
            _projectContext.OrderPayment.Remove(orderPayment);   
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
