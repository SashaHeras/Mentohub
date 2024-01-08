using Mentohub.Domain.Data.Entities;
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

        public string UserId { get; set; }

        public virtual List<TaskHistory> TaskHistory { get; set; }

        [ForeignKey("TestId")]
        public virtual Test Test { get; set; }

        [ForeignKey("UserId")]
        public virtual CurrentUser User { get; set; }

        public TestHistory()
        {

        }

        public TestHistory(double totalMark, DateTime date, int testId, string userId)
        {
            TotalMark = totalMark;
            Date = date;
            TestId = testId;
            this.UserId = userId;
        }
    }
}
