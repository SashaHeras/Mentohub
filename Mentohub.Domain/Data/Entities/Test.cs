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
        public virtual CourseItem CourseItem { get; set; }

        public virtual List<TestTask> TestTasks { get; set; }

        public virtual List<TestHistory> TestHistory { get; set; }
    }
}
