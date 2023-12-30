using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Mentohub.Domain.Entities
{
    public class Course
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PicturePath { get; set; }

        public string? PreviewVideoPath { get; set; }

        public string AuthorId { get; set; }

        public bool Checked { get; set; }

        public double Rating { get; set; }

        public decimal Price { get; set; }

        public int CourseSubjectId { get; set; }

        public int? LanguageID { get; set; }

        public string? LoadVideoName { get; set; }

        public string? LoadPictureName { get; set; }

        public DateTime LastEdittingDate { get; set; }

        public CurrentUser Author { get; set; }

        public List<CourseItem> CourseItems { get; set; }

        public List<Comment> Comments { get; set; }

        public List<CourseViews> CourseViews { get; set; }

        public List<CourseBlock> CourseBlocks { get; set; }

        public CourseLanguage Language { get; set; }

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