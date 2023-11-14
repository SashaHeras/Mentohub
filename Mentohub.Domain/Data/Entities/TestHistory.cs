using System.ComponentModel.DataAnnotations.Schema;

namespace Mentohub.Domain.Entities
{
    public class TestHistory
    {
        public int Id { get; set; }

        public double Mark { get; set; } = 0;

        public double TotalMark { get; set; } = 0;

        public DateTime Date { get; set; }

        public int TestId { get; set; }

        public Guid UserId { get; set; }

        public List<TaskHistory> TaskHistory { get; set; }

        [ForeignKey("TestId")]
        public Test Test { get; set; }

        public TestHistory()
        {

        }

        public TestHistory(double totalMark, DateTime date, int testId, Guid userId)
        {
            TotalMark = totalMark;
            Date = date;
            TestId = testId;
            this.UserId = userId;
        }
    }
}
