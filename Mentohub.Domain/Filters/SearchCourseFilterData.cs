using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Domain.Filters
{
    public class SearchCourseFilterData
    {
        public List<KeyValuePair<int, string>> Categories { get; set; }

        public List<KeyValuePair<int, string>> Languages { get; set; }

        public List<KeyValuePair<int, string>> Levels { get; set; }
    }
}
