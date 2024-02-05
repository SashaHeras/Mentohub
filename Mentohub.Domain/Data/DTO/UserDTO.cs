using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Mentohub.Domain.Data.DTO
{
    public class UserDTO
    {
        public string? Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public List<string>? UserRoles { get; set; }
        
        public string? FirstName { get; set; }
        
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }
        
        public string AboutMe { get; set; }

        public string? EncryptedID { get; set; }

        public DateTime? DateOfBirth { get; set; }

    }
}
