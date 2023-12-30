using Mentohub.Domain.Data.DTO;

namespace Mentohub.Domain.Data.Entities.CourseEntities
{
    public class CourseSubject
    {
        public int Id { get; set; }

        public string Name { get; set; }

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