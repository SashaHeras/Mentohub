using Mentohub.Domain.Data.Entities.Interfaces;
using Mentohub.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentohub.Domain.Data.Entities
{
    [Table("AspNetUsers")]
    public class CurrentUser : IdentityUser, IItem
    {
        [JsonProperty]
        public string? FirstName { get; set; }

        [JsonProperty]
        public string? LastName { get; set; }

        [JsonProperty]
        public string? Image { get; set; }

        [JsonProperty]
        public string? AboutMe { get; set; }

        [JsonProperty]
        public DateTime DateOfBirth { get; set; }

        public List<CourseViews> CourseViews { get; set; }

        public List<Comment> Comments { get; set; }
    }
}
