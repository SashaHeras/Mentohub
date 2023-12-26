using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Domain.Data.DTO
{
    public class UserDTO
    {
        [BindNever]
        public string? Id { get; set; }
        [BindNever]
        public string? Name { get; set; }
        [BindNever]
        public string? Email { get; set; }  
        public List<string>? UserRoles { get; set; }
        [BindNever]
        public string? FirstName { get; set; }
        [BindNever]
        public string? LastName { get; set; }
        [BindNever]
        public string? AboutMe { get; set; }
        [BindNever]
        public DateTime? DateOfBirth { get; set; }

    }
}
