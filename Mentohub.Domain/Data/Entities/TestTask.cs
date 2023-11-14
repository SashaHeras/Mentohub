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
        public Test Test { get; set; }

        public List<TaskAnswer> TaskAnswers { get; set; }

        public List<TaskHistory> TaskHistory { get; set; }
    }
}
