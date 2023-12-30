using Mentohub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Domain.Data.Entities
{
    public class CourseLanguage
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Course> Courses { get; set; }
    }
}
