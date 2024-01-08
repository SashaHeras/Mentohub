using System.ComponentModel.DataAnnotations.Schema;

namespace Mentohub.Domain.Entities
{
    public class AnswerHistory
    {
        public int Id { get; set; }

        public int AnswerId { get; set; }

        public int TaskId { get; set; }

        public int TaskHistoryId { get; set; }

        public bool IsCorrect { get; set; }

        [ForeignKey("TaskHistoryId")]
        public virtual TaskHistory TaskHistory { get; set; }

        [ForeignKey("TaskId")]
        public virtual TestTask TestTask { get; set; }

        [ForeignKey("AnswerId")]
        public virtual TaskAnswer TaskAnswer { get; set; }
    }
}
