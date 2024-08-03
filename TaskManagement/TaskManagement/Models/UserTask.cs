namespace TaskManagement.Models
{
    public class UserTask
    {
        public int UserTaskId { get; set; }
        public Task Task { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
