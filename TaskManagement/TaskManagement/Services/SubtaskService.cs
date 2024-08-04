using TaskManagement.Models;
using TaskManagement.Repositories;

namespace TaskManagement.Services
{
    public class SubtaskService : ISubtaskService
    {
        private readonly IGenericRepository<Subtask> _iSubtaskRepository;
        public SubtaskService(IGenericRepository<Subtask> iSubtaskRepository)
        {
            _iSubtaskRepository = iSubtaskRepository;
        }
        public IEnumerable<Subtask> GetAll()
        {
            return _iSubtaskRepository.GetAll().ToList();
        }

        public void Insert(Subtask subtask)
        {
            _iSubtaskRepository.Insert(subtask);
            _iSubtaskRepository.Save();
        }
    }
}
