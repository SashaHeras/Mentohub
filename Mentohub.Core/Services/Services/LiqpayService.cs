using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.Payment;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Services
{
    public class LiqpayService : ILiqpayService
    {
        private readonly IOrderService _orderService;
        private readonly IOrderItemSevice _orderItemService;
        private readonly IConfiguration _config;

        public LiqpayService(
            IOrderService orderService, 
            IOrderItemSevice orderItemService,
            IConfiguration config
        )
        {
            _orderService = orderService;
            _orderItemService = orderItemService;
            _config = config;
        }

        public LiqPayCheckoutFormModel GenerateOrderPayModel(string orderID)
        {
            var order = _orderService.GetOrder(orderID);

            if(order == null)
            {
                return null;
            }

            var signature_source = new LiqPayCheckout()
            {
                public_key = _config["Payment:Public"],
                version = 3,
                action = "pay",
                amount = (decimal)order.OrderItems.Sum(x => x.Total),
                currency = "UAH",
                description = "Оплата замовлення. Придбані курси: " + string.Join(',', order.OrderItems.Select(x => { return " " + x.Course.Name; })),
                order_id = orderID,
                sandbox = 1,
                result_url = "https://localhost:7236/Home/Redirect?order_id=" + orderID,
            };

            var json_string = JsonConvert.SerializeObject(signature_source);
            var data_hash = Convert.ToBase64String(Encoding.UTF8.GetBytes(json_string));
            var signature_hash = GetLiqPaySignature(data_hash);

            var model = new LiqPayCheckoutFormModel();
            model.Data = data_hash;
            model.Signature = signature_hash;

            return model;
        }

        /// <summary>
        /// Формування сигнатури
        /// </summary>
        /// <param name="data">Json string з параметрами для LiqPay</param>
        /// <returns></returns>
        public string GetLiqPaySignature(string data)
        {
            var buffer = Encoding.UTF8.GetBytes(_config["Payment:Private"] + data + _config["Payment:Private"]);
            return Convert.ToBase64String(SHA1.Create().ComputeHash(buffer));
        }
    }
}
