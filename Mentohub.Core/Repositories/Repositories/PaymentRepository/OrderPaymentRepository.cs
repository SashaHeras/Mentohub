using Mentohub.Core.Repositories.Interfaces.PaymentInterfaces;
using Mentohub.Domain.Data.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Repositories.Repositories.PaymentRepository
{
    public class OrderPaymentRepository : IOrderPaymentRepository
    {
        public ICollection<OrderPayment> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
