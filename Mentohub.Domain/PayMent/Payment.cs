using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Mentohub.Domain.PayMent
{
    public class Payment
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlElement("prop")]
        public List<Prop> Props { get; set; }
        public Payment() 
        { 
            Id=Guid.NewGuid().ToString();
            Seed();
        }
        public void AddToListProps(string name,string value)
        {
            Props.Add(new Prop { Name = name, Value = value });
        }
        public void Seed()
        {
            AddToListProps("b_card_or_acc", "26004520012448");
            AddToListProps("amt", "1");
            AddToListProps("ccy", "UAH");
            AddToListProps("b_name", "test_test");
            AddToListProps("b_crf", "283123814");
            AddToListProps("b_bic", "336310");
            AddToListProps("details", "testUkr");
        }
        
    }
}
