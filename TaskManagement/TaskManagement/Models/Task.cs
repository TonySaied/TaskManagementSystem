using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace TaskManagement.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [DisplayName("Due Date")]
        public DateTime DueDate { get; set; }
        [DisplayName("Attachment")]
        public string AttachmentPath { get; set; }
        [ForeignKey("Project")]
        [DisplayName("Project")]
        public int ProjectId { get; set; }
        public Project? Project { get; set; }
        public List<UserTask> UserTasks { get; set; }
        public List<Subtask>? Subtasks { get; set; }
    }
}
