namespace Mentohub.Domain.Entities
{
    public class CourseElement
    {
        public int CourseItemId { get; set; }

        public int TypeId { get; set; }

        public int CourseId { get; set; }

        public DateTime DateCreation { get; set; }

        public int OrderNumber { get; set; }

        public string ElementName { get; set; }
    }
}
