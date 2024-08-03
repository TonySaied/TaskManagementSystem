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
            var Proj = _context.Projects.Find(id);
            //We Have to delete all assigned tasks first
            var TaskListForProject=_context.Tasks.Where(i=>i.ProjectId==id).ToList();
            if (Proj == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _context.Tasks.RemoveRange(TaskListForProject);//Removes a List
                _context.Projects.Remove(Proj);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

    }
}
