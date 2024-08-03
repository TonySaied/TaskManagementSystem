using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskManagement.Models.ViewModel
{
    public class TaskViewModel
    {
        public Task Task { get; set; }
        public SelectList Projects { get; set; }
        public List<SelectListItem> Users { get; set; }
        public List<int> SelectedUserIds { get; set; }
    }
}
