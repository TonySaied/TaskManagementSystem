using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Models;
using TaskManagement.Models.ViewModel;

namespace TaskManagement.Controllers
{
    public class TaskController : Controller
    {
        ApplicationDbContext _context;
        public TaskController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? userId)
        {
            ViewBag.UsersList = new SelectList(_context.Users, "Id", "Name");

            List<TaskManagement.Models.Task> tasks;

            if (userId.HasValue && userId.Value > 0)
            {
                tasks = _context.Tasks
                    .Include(t => t.UserTasks)
                        .ThenInclude(ut => ut.User)
                    .Where(t => t.UserTasks.Any(ut => ut.UserId == userId.Value))
                    .ToList();
            }
            else
            {
                tasks = _context.Tasks
                    .Include(t => t.UserTasks)
                        .ThenInclude(ut => ut.User)
                    .ToList();
            }

            return View(tasks);
        }


        public IActionResult New()
        {
            // Populate ProjectsList for dropdown
            ViewBag.ProjectsList = new SelectList(_context.Projects, "Id", "Name");
            ViewBag.UsersList = new MultiSelectList(_context.Users, "Id", "Name"); // Add this line for user selection
            return View();
        }

        // POST: Tasks/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(TaskViewModel taskViewModel)
        {
            ViewBag.ProjectsList = new SelectList(_context.Projects, "Id", "Name"); // Re-populate the ProjectsList

            if (ModelState.IsValid)
            {
                var task = new TaskManagement.Models.Task
                {
                    Name = taskViewModel.Name,
                    Description = taskViewModel.Description,
                    DueDate = taskViewModel.DueDate,
                    AttachmentPath = taskViewModel.AttachmentPath,
                    ProjectId = taskViewModel.ProjectId
                };

                _context.Add(task);
                await _context.SaveChangesAsync();

                // Add user-task relationships
                foreach (var userId in taskViewModel.SelectedUserIds)
                {
                    _context.Add(new UserTask { TaskId = task.Id, UserId = userId });
                }
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // If model state is invalid, re-populate lists
            ViewBag.UsersList = new MultiSelectList(_context.Users, "Id", "Name");
            return View(taskViewModel);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var Task = _context.Tasks.Where(e => e.Id == id).FirstOrDefault();
            return View(Task);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            var task = _context.Tasks.Find(id);
            if (task == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _context.Tasks.Remove(task);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
