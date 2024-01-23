using LiqPay.Models;
using MassTransit;
using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Interfaces.PaymentInterfaces;
using Mentohub.Domain.Data.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Currency = Mentohub.Domain.Data.Entities.Order.Currency;

namespace Mentohub.Core.Repositories.Repositories.PaymentRepositories
{
    public class CurrencyRepository : Repository<Currency>, ICurrencyRepository
    {
        private readonly ProjectContext _projectContext;
        public CurrencyRepository(ProjectContext projectContext) : base(projectContext) 
        {
            _projectContext = projectContext;
        }

        
        public void Delete(Currency currency)
        {
            _projectContext.Currency.Remove(currency);
            _projectContext.SaveChanges();
        }

       

        
    }
}
