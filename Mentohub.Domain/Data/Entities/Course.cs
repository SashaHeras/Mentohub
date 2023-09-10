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
    }
}