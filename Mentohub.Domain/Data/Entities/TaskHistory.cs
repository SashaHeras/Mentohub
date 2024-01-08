using System.ComponentModel.DataAnnotations.Schema;

namespace Mentohub.Domain.Entities
{
    public class TaskHistory
    {
        public int Id { get; set; }

        public int TaskId { get; set; }

        public int TestHistoryId { get; set; }

        public double UserMark { get; set; }

        [ForeignKey("TestHistoryId")]
        public virtual TestHistory TestHistory { get; set; }

        [ForeignKey("TaskId")]
        public virtual TestTask TestTask { get; set; }

        public virtual List<AnswerHistory> AnswerHistory { get; set; }
    }
}
