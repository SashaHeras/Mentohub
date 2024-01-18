using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Mentohub.Domain.PayMent
{
    public class Data
    {
        [XmlElement("oper")]
        public string Operation { get; set; }

        [XmlElement("wait")]
        public int Wait { get; set; }

        [XmlElement("test")]
        public int Test { get; set; }

        [XmlElement("payment")]
        public Payment Payment { get; set; }
        public Data ()
        {
            Payment = new Payment();
        }
    }
}
