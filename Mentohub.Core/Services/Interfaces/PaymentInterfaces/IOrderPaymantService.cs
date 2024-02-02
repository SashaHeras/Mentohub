using Mentohub.Domain.Data.DTO.Payment;
using Mentohub.Domain.Data.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Interfaces.PaymentInterfaces
{
    public interface IOrderPaymantService
    {
        ICollection<OrderPaymentDTO> GetOrderPayments(string id);

        bool DeleteOrderPayment(string id);

        Task<OrderPayment> CreateOrderPaymant(decimal total, int currencyID, string orderID);
    }
}
