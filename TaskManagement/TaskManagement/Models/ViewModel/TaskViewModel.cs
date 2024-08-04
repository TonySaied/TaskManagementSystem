using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Models.ViewModel
{
    public class TaskViewModel
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public string AttachmentPath { get; set; }

        [Required]
        public int ProjectId { get; set; }
        public List<int> SelectedUserIds { get; set; } = new List<int>();
        public List<Subtask> Subtasks { get; set; }
    }
}
