using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Domain.Data.DTO
{
    public class AuthorInfoDTO
    {
        public string? Id { get; set; }
        public string? Image { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }   
        public string? AboutMe { get; set; }
        public double? AvarageRating { get; set; }
        public int? CountOfStudents { get; set; }
    }
}
