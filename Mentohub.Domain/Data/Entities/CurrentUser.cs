using Mentohub.Domain.Data.Entities.CourseEntities;
using Mentohub.Domain.Data.Entities.Interfaces;
using Mentohub.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentohub.Domain.Data.Entities
{
    [Table("AspNetUsers")]
    public class CurrentUser : IdentityUser
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
        public DateTime? DateOfBirth { get; set; }

        public virtual List<CourseViews> CourseViews { get; set; }

        public virtual List<Comment> Comments { get; set; }

        public virtual List<Course> Courses { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
        public virtual  ICollection<UserCourse>? UserCourses { get; set; }
    }
}
