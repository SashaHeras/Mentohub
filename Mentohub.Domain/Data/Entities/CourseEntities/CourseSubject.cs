using Mentohub.Domain.Data.DTO.CourseDTOs;
using System.Text.Json.Serialization;

namespace Mentohub.Domain.Data.Entities.CourseEntities
{
    public class CourseSubject
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Course> Courses { get; set; }

        public CourseSubjectDTO ToDTO()
        {
            return new CourseSubjectDTO
            {
                Id = Id,
                Name = Name
            };
        }
    }
}