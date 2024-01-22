using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace Mentohub.Domain.Payment
{
    public class LiqPayHelper
    {
        static public string _private_key;
        static public readonly string _public_key;

        static LiqPayHelper()
        {
            _public_key = "sandbox_i12181666709";
            _private_key = "sandbox_ipTb02OFMs7IETrDTdKRKuUwnn3EWHJPFQLXvJCv";
        }

        /// <summary>
        /// Сформувати дані для LiqPay (data, signature)
        /// </summary>
        /// <param name="order_id">Номер замовлення</param>
        /// <returns></returns>
        static public LiqPayCheckoutFormModel GetLiqPayModel(string order_id)
        {
            var signature_source = new LiqPayCheckout()
            {
                public_key = _public_key,
                version = 3,
                action = "pay",
                amount = 1,
                currency = "UAH",
                description = "Оплата замовлення",
                order_id = order_id,
                sandbox = 1,

                result_url = "https://localhost:7218/Home/Redirect?order_id=" + order_id,

                product_category = "Напої",
                product_description = "Гаряче какао з альпійським молоком",
                product_name = "Гаряче какао"
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
        static public string GetLiqPaySignature(string data)
        {
            var buffer = Encoding.UTF8.GetBytes(_private_key + data + _private_key);
            return Convert.ToBase64String(SHA1.Create().ComputeHash(buffer));
        }
    }
}
