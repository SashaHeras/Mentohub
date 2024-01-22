using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.Payment;
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

        private readonly string _public_key = "sandbox_i12181666709";
        private readonly string _private_key = "sandbox_ipTb02OFMs7IETrDTdKRKuUwnn3EWHJPFQLXvJCv";

        public LiqpayService(
            IOrderService orderService, 
            IOrderItemSevice orderItemService
        )
        {
            _orderService = orderService;
            _orderItemService = orderItemService;
        }

        public LiqPayCheckoutFormModel GenerateOrderPayModel(string orderID)
        {
            var order = _orderService.GetOrder(orderID);

            var signature_source = new LiqPayCheckout()
            {
                public_key = _public_key,
                version = 3,
                action = "pay",
                amount = order.OrderItems.Sum(x => x.Total),
                currency = "UAH",
                description = "Оплата замовлення",
                order_id = orderID,
                sandbox = 1,

                result_url = "https://localhost:7218/Home/Redirect?order_id=" + orderID,

                product_category = "Покупка курсів",
                product_description = "До покупки курси: " + string.Join(',', order.OrderItems.Select(x => { return " " + x.Course.Name; }))
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
            var buffer = Encoding.UTF8.GetBytes(_private_key + data + _private_key);
            return Convert.ToBase64String(SHA1.Create().ComputeHash(buffer));
        }
    }
}
