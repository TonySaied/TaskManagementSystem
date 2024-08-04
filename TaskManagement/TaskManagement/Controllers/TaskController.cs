using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManagement.Models;
using TaskManagement.Models.ViewModel;
using TaskManagement.Services;

namespace TaskManagement.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly ISubtaskService _SubtaskService;

        public TaskController(ITaskService taskService, ISubtaskService SubtaskService)
        {
            _taskService = taskService;
            _SubtaskService = SubtaskService;
        }

        public IActionResult Index(int? userId)
        {
            ViewBag.UsersList = new SelectList(_taskService.GetUsers(), "Id", "Name");

            var tasks = _taskService.GetAll();

            if (userId.HasValue && userId.Value > 0)
            {
                tasks = tasks.Where(t => t.UserTasks.Any(ut => ut.UserId == userId.Value)).ToList();
            }

            return View(tasks);
        }

        public IActionResult New()
        {
            ViewBag.ProjectsList = new SelectList(_taskService.GetProjects(), "Id", "Name");
            ViewBag.UsersList = new MultiSelectList(_taskService.GetUsers(), "Id", "Name");
            ViewBag.SubtasksList = new SelectList(_SubtaskService.GetAll(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult New(TaskViewModel taskViewModel)
        {
            ViewBag.ProjectsList = new SelectList(_taskService.GetProjects(), "Id", "Name");
            ViewBag.UsersList = new MultiSelectList(_taskService.GetUsers(), "Id", "Name");
            ViewBag.SubtasksList = new SelectList(_SubtaskService.GetAll(), "Id", "Name");

            if (ModelState.IsValid)
            {
                var task = new TaskManagement.Models.Task
                {
                    Name = taskViewModel.Name,
                    Description = taskViewModel.Description,
                    DueDate = taskViewModel.DueDate,
                    AttachmentPath = taskViewModel.AttachmentPath,
                    ProjectId = taskViewModel.ProjectId,
                    Subtasks = taskViewModel.Subtasks 
                };

                _taskService.Insert(task, taskViewModel.SelectedUserIds);

                return RedirectToAction("Index");
            }

            return View(taskViewModel);
        }


        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var task = _taskService.GetById(id.Value);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _taskService.Delete(id.Value);
            return RedirectToAction(nameof(Index));
        }
    }
}
