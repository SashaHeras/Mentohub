namespace Mentohub.Domain.Entities
{
    public class TaskHistory
    {
        public int Id { get; set; }

        public int TaskId { get; set; }

        public int TestHistoryId { get; set; }

        public double UserMark { get; set; }
    }
}
