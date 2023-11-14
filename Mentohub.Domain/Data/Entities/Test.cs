namespace Mentohub.Domain.Entities
{
    public class Test
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CourseItemId { get; set; }

        public CourseItem CourseItem { get; set; }

        public List<TestTask> TestTasks { get; set; }

        public List<TestHistory> TestHistory { get; set; }
    }
}
