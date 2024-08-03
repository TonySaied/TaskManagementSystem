using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace TaskManagement.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<UserTask>? UserTasks { get; set; }

    }
}
