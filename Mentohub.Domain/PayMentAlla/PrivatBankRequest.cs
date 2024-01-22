using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Mentohub.Domain.PayMentAlla
{
    [XmlRoot("request")]
    public class PrivatBankRequest
    {
        [XmlElement("merchant")]
        public Merchant Merchant { get; set; }

        [XmlElement("data")]
        public Data Data { get; set; }
        public PrivatBankRequest(string id,string signature) 
        {
            Data=new Data();
            Merchant=new Merchant(id, signature);       
        }
    }
}
