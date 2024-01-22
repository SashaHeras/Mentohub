using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Core.Repositories.Interfaces;
using Mentohub.Core.Repositories.Interfaces.PaymentInterfaces;
using Mentohub.Domain.Data.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Repositories.Repositories.PaymentRepository
{
    public class OrderRepository: Repository<Order>, IOrderRepository
    {
#pragma warning disable 8603
        private readonly ProjectContext _projectContext;
        
        public OrderRepository(ProjectContext projectContext) : base(projectContext)
        {
            _projectContext = projectContext;
        }

        public Order GetOrder(string id)
        {
            return GetAll().Where(o=>o.ID == id).FirstOrDefault();
        }

        public void AddOrder(Order order)
        {
            _projectContext.Order.Add(order);
            _projectContext.SaveChanges();
        }

        public void UpdateOrder(Order order)
        {
            _projectContext.Update(order);
            _projectContext.SaveChanges();
        }

        public void DeleteOrder(Order order) 
        {
            _projectContext.Remove(order);
            _projectContext.SaveChanges();
        }
    }
}
