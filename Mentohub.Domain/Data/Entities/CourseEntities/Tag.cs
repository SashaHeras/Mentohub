namespace Mentohub.Domain.Data.Entities.CourseEntities
{
    public class Tag
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public bool Enabled { get; set; }

        public DateTime Created { get; set; }

        public string? UserID { get; set; }

        public virtual ICollection<CourseTag> CourseTags { get; set; }

        public virtual CurrentUser? User { get; set; }
    }
}
