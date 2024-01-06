using System.ComponentModel.DataAnnotations.Schema;
using Mentohub.Domain.Data.Entities.CourseEntities;

namespace Mentohub.Domain.Entities
{
    public class Test
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CourseItemId { get; set; }

        [ForeignKey("CourseItemId")]
        public CourseItem CourseItem { get; set; }

        public List<TestTask> TestTasks { get; set; }

        public List<TestHistory> TestHistory { get; set; }
    }
}
