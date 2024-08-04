using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TaskManagement.Models
{
    public class Subtask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [DisplayName("Due Date")]
        public DateTime DueDate { get; set; }

        [ForeignKey("Task")]
        public int? TaskId { get; set; }
        public Task? Task { get; set; }
    }
}
