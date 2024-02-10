using Mentohub.Core.Services.Services.PaymentServices;
using Mentohub.Domain.Payment;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Interfaces
{
    public interface ILiqpayService : IService
    {
        public LiqPayCheckoutFormModel GenerateOrderPayModel(string orderID);

        /// <summary>
        /// Формування сигнатури
        /// </summary>
        /// <param name="data">Json string з параметрами для LiqPay</param>
        /// <returns></returns>
        public string GetLiqPaySignature(string data);
    }
}
