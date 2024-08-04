using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Models;
using TaskManagement.Repositories;

namespace TaskManagement.Services
{
    public class ProjectService:IProjectService
    {
        private readonly IGenericRepository<TaskManagement.Models.Project> _iProjectRepository;
        private readonly IGenericRepository<TaskManagement.Models.Task> _iTaskRepository;
        private readonly IGenericRepository<UserTask> _iUserTaskRepository;
        public ProjectService(IGenericRepository<TaskManagement.Models.Project> iProjectRepository,
            IGenericRepository<TaskManagement.Models.Task> iTaskRepository,
            IGenericRepository<UserTask> iUserTaskRepository)
        {
            _iProjectRepository = iProjectRepository;
            _iTaskRepository = iTaskRepository;
            _iUserTaskRepository = iUserTaskRepository;
        }

        public void Delete(int id)
        {
            var project = _iProjectRepository.GetById(id);
            if (project == null) return;

            // Retrieve all tasks for this project
            var tasksForProject = _iTaskRepository.GetAll()
                .Where(t => t.ProjectId == id)
                .ToList();

            // Get all UserTasks for the tasks of this project
            var taskIds = tasksForProject.Select(t => t.Id).ToList();
            var userTasksForProject = _iUserTaskRepository.GetAll()
                .Where(ut => taskIds.Contains(ut.TaskId))
                .ToList();

            // Delete related records
            _iUserTaskRepository.DeleteRange(userTasksForProject);
            _iTaskRepository.DeleteRange(tasksForProject);
            _iProjectRepository.Delete(id);

            // Save changes
            _iProjectRepository.Save();
        }

        public IEnumerable<TaskManagement.Models.Project> GetAll()
        {
            return _iProjectRepository.GetAll().Include(x => x.Tasks).ToList();//Eager Loading to Load the tasks assigned to a Project
        }

        public Project GetById(int id)
        {
            return _iProjectRepository.GetById(id);
        }

        public void Insert(Project project)
        {
            _iProjectRepository.Insert(project);
            _iProjectRepository.Save();
        }

        public void Update(Project project)
        {
            var ProjId = new SqlParameter("@ID", project.Id);
            var ProjName = new SqlParameter("@ProjectName", project.Name);
            var ProjDescription = new SqlParameter("@ProjectDescription", project.Description);
            _iProjectRepository.ExecuteRawSql("Exec UpdateProject @ID, @ProjectName, @ProjectDescription",
                ProjId, ProjName, ProjDescription);
        }
    }
}
