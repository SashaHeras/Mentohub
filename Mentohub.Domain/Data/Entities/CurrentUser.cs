using Mentohub.Domain.Data.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Mentohub.Domain.Data.Entities
{
    public class CurrentUser: IdentityUser, IItem
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Image { get; set; }
        public string? AboutMe { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
