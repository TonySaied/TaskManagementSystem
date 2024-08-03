namespace TaskManagement.Models
{
    public class UserTask
    {
        public int id { get; set; }
        public int TaskId { get; set; }
        public Task Task { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
