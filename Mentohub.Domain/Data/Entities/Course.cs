using Mentohub.Domain.Data.DTO;

namespace Mentohub.Domain.Entities
{
    public class Course
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PicturePath { get; set; }

        public string? PreviewVideoPath { get; set; }

        public Guid AuthorId { get; set; }

        public bool Checked { get; set; }

        public double Rating { get; set; }

        public decimal Price { get; set; }

        public int? CourseSubjectId { get; set; }

        public DateTime LastEdittingDate { get; set; }
    
        public CourseDTO ToDTO()
        {
            return new CourseDTO
            {
                Id = Id,
                Name = Name,
                PicturePath = PicturePath,
                PreviewVideoPath = PreviewVideoPath,
                AuthorId = AuthorId,
                Checked = Checked,
                Rating = Rating,
                Price = Price,
                LastEdittingDate = LastEdittingDate
            };
        }
    }
}