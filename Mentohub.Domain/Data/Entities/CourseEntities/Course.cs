using Mentohub.Domain.Data.DTO.CourseDTOs;
using Mentohub.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Mentohub.Domain.Data.Entities.CourseEntities
{
    public class Course
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PicturePath { get; set; }

        public string? PreviewVideoPath { get; set; }

        public string AuthorId { get; set; }

        [MaxLength(200)]
        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public bool Checked { get; set; }

        public double Rating { get; set; }

        public decimal Price { get; set; }

        public int CourseSubjectId { get; set; }

        public int? LanguageID { get; set; }

        public int? CourseLevelID { get; set; }

        public string? LoadVideoName { get; set; }

        public string? LoadPictureName { get; set; }

        public DateTime LastEdittingDate { get; set; }

        public virtual CurrentUser Author { get; set; }

        public virtual CourseSubject Category { get; set; }

        public virtual List<CourseItem> CourseItems { get; set; }

        public virtual ICollection<CourseTag> CourseTags { get; set; }

        public virtual List<Comment> Comments { get; set; }

        public virtual List<CourseViews> CourseViews { get; set; }

        public virtual List<CourseBlock> CourseBlocks { get; set; }

        public virtual List<CourseOverview> CourseOverviews { get; set; }

        public virtual CourseLanguage Language { get; set; }

        public virtual CourseLevel CourseLevel { get; set; }

        public CourseDTO ToDTO()
        {
            return new CourseDTO
            {
                Id = Id,
                Name = Name,
                PicturePath = PicturePath,
                PreviewVideoPath = PreviewVideoPath,
                AuthorId = AuthorId.ToString(),
                Checked = Checked,
                Rating = Rating,
                Price = Price,
                LastEdittingDate = LastEdittingDate
            };
        }
    }
}