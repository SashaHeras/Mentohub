using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Domain.Data.Models
{
    public class AdditionalListModel
    {
        public List<KeyValuePair<int, string>> Categories { get; set; } = new List<KeyValuePair<int, string>>();

        public List<KeyValuePair<int, string>> Levels { get; set; } = new List<KeyValuePair<int, string>>();

        public List<KeyValuePair<int, string>> Languages { get; set; } = new List<KeyValuePair<int, string>>();
    }
}
