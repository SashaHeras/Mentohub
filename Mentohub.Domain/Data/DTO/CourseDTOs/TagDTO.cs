using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Domain.Data.DTO.CourseDTOs
{
    public class TagDTO
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public bool Enabled { get; set; }

        public DateTime Created { get; set; }

        public string UserID { get; set; }
    }
}
