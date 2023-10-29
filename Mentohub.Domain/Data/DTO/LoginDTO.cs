using Mentohub.Domain.Data.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Mentohub.Domain.Data.DTO
{
    public class LoginDTO:IItem
    {
        //public string ReturnUrl { get; set; }

        [Required]
        [Display(Name = "Email")]

        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

    }
}
