using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using TaskManagement.Models;
using Microsoft.EntityFrameworkCore;


namespace TaskManagement.Controllers
{
    public class ProjectController : Controller
    {
        ApplicationDbContext _context;
        public ProjectController(ApplicationDbContext context)
        {
            _context = context;
        }
        //GetAll using Stored Procedure
        public IActionResult Index()
        {
            ///var ProjectsList = _context.Projects.FromSqlRaw("Exec GetAllProjects").ToList();
            //return View(ProjectsList);
            var ProjectsList = _context.Projects.Include(p => p.Tasks).ToList();//Eager Loading to Load the tasks assigned to a Project
            return View(ProjectsList);
        }
        public IActionResult New()
        {
            return View();
        }
        [HttpPost]
        public IActionResult New(Project Proj)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Proj);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Update(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var Emp = _context.Projects.Where(e => e.Id == id).FirstOrDefault();
            return View(Emp);
        }
        //Update using Stored Procedure
        [HttpPost]
        public IActionResult Update(Project Proj)
        {
            if (ModelState.IsValid)
            {
                var ProjId = new SqlParameter("@ID", Proj.Id);
                var ProjName = new SqlParameter("@ProjectName", Proj.Name);
                var ProjDescription = new SqlParameter("@ProjectDescription", Proj.Description);
                _context.Database.ExecuteSqlRaw("Exec UpdateProject @ID, @ProjectName, @ProjectDescription",
                    ProjId, ProjName, ProjDescription);
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
            var Proj = _context.Projects.Where(e => e.Id == id).FirstOrDefault();
            return View(Proj);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Find the project including its related tasks
            var project = _context.Projects.Include(p => p.Tasks).FirstOrDefault(p => p.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            // Retrieve all tasks for this project
            var tasksForProject = project.Tasks.ToList();

            // Get all UserTasks for the tasks of this project
            var userTasksForProject = _context.UserTasks
                .Where(UserTasks => tasksForProject.Select(t => t.Id).Contains(UserTasks.TaskId))
                .ToList();

            _context.UserTasks.RemoveRange(userTasksForProject);// First remove all UserTasks
            _context.Tasks.RemoveRange(tasksForProject);// Remove all tasks for this project
            _context.Projects.Remove(project);// Finally remove project

            _context.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
