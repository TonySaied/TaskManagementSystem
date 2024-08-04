using TaskManagement.Models;
using TaskManagement.Repositories;
using Microsoft.EntityFrameworkCore;

namespace TaskManagement.Services
{
    public class TaskService : ITaskService
    {
        private readonly IGenericRepository<TaskManagement.Models.Task> _taskRepository;
        private readonly IGenericRepository<UserTask> _userTaskRepository;
        private readonly IGenericRepository<Project> _projectRepository;
        private readonly IGenericRepository<User> _userRepository;

        public TaskService(
            IGenericRepository<TaskManagement.Models.Task> taskRepository,
            IGenericRepository<UserTask> userTaskRepository,
            IGenericRepository<Project> projectRepository,
            IGenericRepository<User> userRepository)
        {
            _taskRepository = taskRepository;
            _userTaskRepository = userTaskRepository;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
        }

        public void Delete(int id)
        {
            var task = _taskRepository.GetById(id);
            if (task == null) return;

            // Retrieve all UserTasks for the task
            var userTasksForTask = _userTaskRepository.GetAll()
                .Where(ut => ut.TaskId == id)
                .ToList();

            // Delete related records
            _userTaskRepository.DeleteRange(userTasksForTask);
            _taskRepository.Delete(id);

            // Save changes
            _taskRepository.Save();
        }

        public IEnumerable<TaskManagement.Models.Task> GetAll()
        {
            return _taskRepository.GetAll().Include(t => t.UserTasks).ThenInclude(ut => ut.User).ToList();
        }

        public TaskManagement.Models.Task GetById(int id)
        {
            return _taskRepository.GetById(id);
        }

        public void Insert(TaskManagement.Models.Task task, IEnumerable<int> userIds)
        {
            _taskRepository.Insert(task);
            _taskRepository.Save();

            foreach (var userId in userIds)
            {
                _userTaskRepository.Insert(new UserTask { TaskId = task.Id, UserId = userId });
            }
            _userTaskRepository.Save();
        }

        public IEnumerable<Project> GetProjects()
        {
            return _projectRepository.GetAll().ToList();
        }

        public IEnumerable<User> GetUsers()
        {
            return _userRepository.GetAll().ToList();
        }
    }
}
