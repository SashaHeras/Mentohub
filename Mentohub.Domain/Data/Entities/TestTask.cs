using System.ComponentModel.DataAnnotations.Schema;

namespace Mentohub.Domain.Entities
{
    public class TestTask
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Mark { get; set; }

        public int TestId { get; set; }

        public int OrderNumber { get; set; }

        public bool IsFewAnswersCorrect { get; set; }

        [ForeignKey("TestId")]
        public virtual Test Test { get; set; }

        public virtual List<TaskAnswer> TaskAnswers { get; set; }

        public virtual List<TaskHistory> TaskHistory { get; set; }
    }
}
