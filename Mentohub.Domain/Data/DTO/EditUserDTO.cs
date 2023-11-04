using Mentohub.Domain.Data.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Domain.Data.DTO
{
    public class EditUserDTO:IItem
    {
        //public string? Id { get; set; }
        //public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? AboutMe { get; set; }
        //public DateTime DateOfBirth { get; set; }
     
    }
}
