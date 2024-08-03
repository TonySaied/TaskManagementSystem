using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace TaskManagement.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("Task")]
        [DisplayName("Task")]
        public int? TaskId { get; set; }
        public Task? Task { get; set; }

    }
}
