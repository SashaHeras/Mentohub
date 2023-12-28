using Mentohub.Domain.Data.Entities;

namespace Mentohub.Domain.Entities
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
        
        public Course Course { get; set; }

        public CourseBlock CourseBlock { get; set; }

        public Lesson Lesson { get; set; }

        public Test Test { get; set; }
    }
}
