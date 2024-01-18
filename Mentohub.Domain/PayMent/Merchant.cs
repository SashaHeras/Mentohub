using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Mentohub.Domain.PayMent
{
    public class Merchant
    {
        [XmlElement("id")]
        public string Id { get; set; }

        [XmlElement("signature")]
        public string Signature { get; set; }
        public Merchant() { }
        public Merchant(string Id, string Signature) 
        {
            this.Id = Id;
            this.Signature = Signature;
        }   
    }
}
