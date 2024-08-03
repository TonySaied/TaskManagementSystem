namespace TaskManagement.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //Nullable because there maybe a Project With no Tasks yet
        public List<Task>? Tasks { get; set; }
    }
}
