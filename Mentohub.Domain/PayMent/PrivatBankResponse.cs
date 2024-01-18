using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Mentohub.Domain.PayMent
{
    [XmlRoot("response")]
    public class PrivatBankResponse
    {
        [XmlElement("merchant")]
        public Merchant Merchant { get; set; }

        [XmlElement("data")]
        public DataResponse Data { get; set; }
    }
}
