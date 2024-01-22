using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Mentohub.Domain.PayMentAlla
{
    public class Data
    {
        [XmlElement("oper")]
        public string Operation { get; set; }

        [XmlElement("wait")]
        public int Wait { get; set; }

        [XmlElement("test")]
        public int Test { get; set; }

        [XmlElement("PayMentAlla")]
        public PayMentAlla PayMentAlla { get; set; }
        public Data ()
        {
            PayMentAlla = new PayMentAlla();
        }
    }
}
