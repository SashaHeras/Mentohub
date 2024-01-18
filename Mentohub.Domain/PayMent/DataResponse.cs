using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Mentohub.Domain.PayMent
{
    public class DataResponse
    {
        [XmlElement("oper")]
        public string Operation { get; set; }

        [XmlElement("payment")]
        public PaymentResponse Payment { get; set; }
    }
}
