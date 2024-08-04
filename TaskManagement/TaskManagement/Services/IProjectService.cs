namespace TaskManagement.Services
{
    public interface IProjectService
    {
        IEnumerable<TaskManagement.Models.Project> GetAll(); //I Have to write the long form because 'Project' causes confusion
        TaskManagement.Models.Project GetById(int id);

        void Insert(TaskManagement.Models.Project project);
        void Update(TaskManagement.Models.Project project);
        void Delete(int id);
    }
}
