namespace TaskManagement.Services
{
    public interface ISubtaskService
    {
        IEnumerable<TaskManagement.Models.Subtask> GetAll();
        void Insert(TaskManagement.Models.Subtask subtask);
    }
}
