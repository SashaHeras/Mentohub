using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace Mentohub.Domain.Payment
{
    public class LiqPayRequest
    {
        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("public_key")]
        public string PublicKey { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("card_token")]
        public string CardToken { get; set; }

        [JsonProperty("ip")]
        public string IP { get; set; }

        [JsonProperty("prepare")]
        public string Prepare { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("sandbox")]
        public string Sandbox { get; set; }

        [JsonIgnore]
        public bool IsSandbox
        {
            get
            {
                return Sandbox == "1";
            }
            set
            {
                Sandbox = value ? "1" : null;
            }
        }

        [JsonProperty("order_id")]
        public string OrderId { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("subscribe_periodicity")]
        public string SubscribePeriodicity { get; set; }

        public string Subscribe { get; set; }

        [JsonIgnore]
        public bool IsSubscribe
        {
            get { return Subscribe == "1"; }
            set { Subscribe = value ? "1" : "0"; }
        }

        [JsonProperty("subscribe_date_start")]
        public string SubscribeDateStart { get; set; }

        [JsonProperty("result_url")]
        public string ResultUrl { get; set; }

        [JsonProperty("server_url")]
        public string ServerUrl { get; set; }

        [JsonIgnore]
        public IDictionary<string, string> OtherParams { get; set; } = new Dictionary<string, string>();
    }
}
