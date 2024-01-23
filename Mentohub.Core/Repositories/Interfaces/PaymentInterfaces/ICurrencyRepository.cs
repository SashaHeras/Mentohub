using LiqPay.Models;
using Mentohub.Core.Infrastructure;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Domain.Data.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Currency = Mentohub.Domain.Data.Entities.Order.Currency;

namespace Mentohub.Core.Repositories.Interfaces.PaymentInterfaces
{
    public interface ICurrencyRepository:IRepository<Currency>,ISingletoneService
    { 
        void Delete(Currency currency);
   
    }
}
