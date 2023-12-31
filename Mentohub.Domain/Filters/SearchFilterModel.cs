using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Domain.Filters
{
    public class SearchFilterModel
    {
        public List<int> Categories { get; set; } = new List<int>();

        public int LanguageID { get; set; }

        public double Rate { get; set; }

        public int PriceFrom { get; set; }

        public int PriceTo { get; set; }
    }
}
