using Mentohub.Domain.Data.DTO.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Currency = Mentohub.Domain.Data.Entities.Order.Currency;

namespace Mentohub.Core.Services.Interfaces.PaymentInterfaces
{
    public interface ICurrencyService
    {
        Currency GetCurrency(int id);

        Currency CreateCurrency(CurrencyDTO currencyDTO);

        bool DeleteCurrency(int id);
    }
}
