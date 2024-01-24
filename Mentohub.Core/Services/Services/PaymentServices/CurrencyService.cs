﻿using Mentohub.Core.Repositories.Interfaces.PaymentInterfaces;
using Mentohub.Core.Services.Interfaces.PaymentInterfaces;
using Mentohub.Domain.Data.DTO.Payment;
using Mentohub.Domain.Data.Entities.Order;


namespace Mentohub.Core.Services.Services.PaymentServices
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currentRepository;

        public CurrencyService(ICurrencyRepository currentRepository)
        {
            _currentRepository = currentRepository;
        }

        public Currency CreateCurrency(CurrencyDTO currencyDTO)
        {
            var currency = _currentRepository.Add(new Currency()
            {
                ID = currencyDTO.ID,
                Name = currencyDTO.Name,
                Code = currencyDTO.Code
            });

            return currency;
        }

        public bool DeleteCurrency(int id)
        {
            var currency = _currentRepository.FirstOrDefault(x => x.ID == id);
            if(currency == null)
            {
                throw new ArgumentNullException(nameof(currency), "Currency does not exist!");
            }

            _currentRepository.Delete(currency);
            return true;
        }

        public Currency GetCurrency(int id)
        {
            var currency = _currentRepository.FirstOrDefault(x => x.ID == id);
            if (currency == null)
            {
                throw new ArgumentNullException(nameof(currency), "Currency does not exist!");
            }

            return currency;
        }

        // Todo: отримати валюту по коду
    }
}