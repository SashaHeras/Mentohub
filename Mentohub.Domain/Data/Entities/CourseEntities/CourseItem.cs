using Mentohub.Domain.Entities;

namespace Mentohub.Domain.Data.Entities.CourseEntities
{
    public class CourseItem
    {
        public int Id { get; set; }

        public int TypeId { get; set; }

        public int CourseId { get; set; }
        public int CourseBlockID { get; set; }

        public DateTime DateCreation { get; set; }

        public int OrderNumber { get; set; }

        public int StatusId { get; set; }

        public virtual Course Course { get; set; }

        public virtual CourseBlock CourseBlock { get; set; }

        public virtual Lesson Lesson { get; set; }

        public virtual Test Test { get; set; }
    }
}
