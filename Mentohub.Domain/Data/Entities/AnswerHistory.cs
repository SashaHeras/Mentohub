namespace Mentohub.Domain.Entities
{
    public class AnswerHistory
    {
        public int Id { get; set; }

        public int AnswerId { get; set; }

        public int TaskId { get; set; }

        public int TaskHistoryId { get; set; }

        public bool IsCorrect { get; set; }
    }
}
