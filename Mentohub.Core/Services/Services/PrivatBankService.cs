using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.PayMent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Mentohub.Core.Services.Services
{
    public class PrivatBankService : IPrivatBankService
    {
        private readonly HttpClient _httpClient;

        public PrivatBankService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PrivatBankResponse> MakePaymentAsync(PrivatBankRequest request)
        {
            var xmlSerializer = new XmlSerializer(typeof(PrivatBankResponse));

            using (var memoryStream = new MemoryStream())
            {
                xmlSerializer.Serialize(memoryStream, request);
                memoryStream.Seek(0, SeekOrigin.Begin);

                var content = new StreamContent(memoryStream);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/xml");

                var response = await _httpClient.PostAsync("https://api.privatbank.ua/p24api/pay_ua", content);

                if (!response.IsSuccessStatusCode)
                {
                    // Обробте помилку тут
                    throw new Exception($"Request failed with status code {response.StatusCode}");
                }

                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    return (PrivatBankResponse)xmlSerializer.Deserialize(responseStream);
                }
            }
        }
    }
}
