using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Domain.Data.DTO.Test
{
    public class PassTestResultDTO
    {
        public int CourseID { get; set; }

        public double Mark { get; set; }

        public double TotalMark { get; set; }

        public int TestID { get; set; }
    }
}
