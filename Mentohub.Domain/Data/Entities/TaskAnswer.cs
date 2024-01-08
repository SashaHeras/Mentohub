using Mentohub.Domain.Data.DTO;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentohub.Domain.Entities
{
    public class TaskAnswer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int TaskId { get; set; }

        public bool IsCorrect { get; set; }

        [ForeignKey("TaskId")]
        public virtual TestTask TestTask { get; set; }

        public virtual List<AnswerHistory> AnswerHistory { get; set; }

        public AnswerDTO ToDTO()
        {
            return new AnswerDTO
            {
                TaskId = TaskId,
                Name = Name,
                Id = Id,
                IsChecked = IsCorrect
            };
        }
    }
}
