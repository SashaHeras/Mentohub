namespace Mentohub.Domain.Entities
{
    public class CourseItem
    {
        public int Id { get; set; }

        public int TypeId { get; set; }

        public int CourseId { get; set; }

        public DateTime DateCreation { get; set; }

        public int OrderNumber { get; set; }

        public int StatusId { get; set; }        
    }
}
