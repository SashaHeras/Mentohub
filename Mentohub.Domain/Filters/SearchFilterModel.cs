using Mentohub.Domain.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Domain.Filters
{
    public class SearchFilterModel
    {
        public string SearchText { get; set; } = string.Empty;

        public int CategoryID { get; set; } = -1;

        public int LanguageID { get; set; } = -1;

        public double Rate { get; set; } = -1;

        public int Level { get; set; } = -1;

        public int PriceFrom { get; set; } = 0;

        public int PriceTo { get; set; } = 500;

        public int CurrentPage { get; set; } = 0;

        public int Count { get; set; } = 10;

        public int SortOption { get; set; } = (int)e_SortOptions.None;
    }
}
