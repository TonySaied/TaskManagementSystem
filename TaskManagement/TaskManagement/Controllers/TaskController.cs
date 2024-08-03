using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public IActionResult Index()
        {
            var Tasks = _context.Tasks.ToList();
            return View(Tasks);
        }
        public IActionResult New()
        {
            ViewBag.ProjectsList = new SelectList(_context.Projects, "Id", "Name");
            return View();
        }
        [HttpPost]
        public IActionResult New(Models.Task task)
        {
            ViewBag.ProjectsList = new SelectList(_context.Projects, "Id", "Name");// Will Display Name but saves Id in Task Table
            if (ModelState.IsValid)
            {
                _context.Add(task);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
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
