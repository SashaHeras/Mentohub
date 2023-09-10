namespace Mentohub.Domain.Entities
{
    public class TaskAnswer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int TaskId { get; set; }

        public bool IsCorrect { get; set; }
    }
}
