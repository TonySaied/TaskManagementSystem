using TaskManagement.Models;
using TaskManagement.Repositories;

namespace TaskManagement.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<TaskManagement.Models.User> _iUserRepository;
        public UserService(IGenericRepository<User> iUserRepository)
        {
            _iUserRepository = iUserRepository;
        }

        public IEnumerable<User> GetAll()
        {
            return _iUserRepository.GetAll().ToList();
        }
    }
}
