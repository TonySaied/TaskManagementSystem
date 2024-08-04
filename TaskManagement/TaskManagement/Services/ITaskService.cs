using TaskManagement.Models;

namespace TaskManagement.Services
{
    public interface ITaskService
    {
        void Delete(int id);
        IEnumerable<TaskManagement.Models.Task> GetAll();
        TaskManagement.Models.Task GetById(int id);
        void Insert(TaskManagement.Models.Task task, IEnumerable<int> userIds);
        IEnumerable<Project> GetProjects();
        IEnumerable<User> GetUsers();
    }
}
