using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Mentohub.Domain.PayMent
{
    public class PaymentResponse
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("state")]
        public int State { get; set; }

        [XmlAttribute("message")]
        public string Message { get; set; }

        [XmlAttribute("ref")]
        public string Ref { get; set; }

        [XmlAttribute("amt")]
        public decimal Amount { get; set; }

        [XmlAttribute("ccy")]
        public string Currency { get; set; }

        [XmlAttribute("comis")]
        public decimal Commission { get; set; }

        [XmlAttribute("code")]
        public string Code { get; set; }

        [XmlAttribute("cardinfo")]
        public string CardInfo { get; set; }
    }
}
