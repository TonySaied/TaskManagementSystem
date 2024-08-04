namespace TaskManagement.Services
{
    public interface IUserService
    {
        IEnumerable<TaskManagement.Models.User> GetAll();
    }
}
