using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using TaskManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using TaskManagement.Services;
using Newtonsoft.Json;


namespace TaskManagement.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        IProjectService _projectService;
        private readonly HttpClient _httpClient;
        public ProjectController(IProjectService projectService, IHttpClientFactory httpClientFactory)
        {
           _projectService = projectService;
            _httpClient = httpClientFactory.CreateClient();
        }
        
        public IActionResult Index()
        {
            //GetAll using Stored Procedure
            ///var ProjectsList = _context.Projects.FromSqlRaw("Exec GetAllProjects").ToList();
            //return View(ProjectsList);

            var ProjectsList = _projectService.GetAll();

            return View(ProjectsList);
        }
        //Calls Web API https://localhost:7124/Project/Index2
        public async Task<IActionResult> Index2()
        {
            var response = await _httpClient.GetAsync("https://localhost:7124/api/ProjectsAPI/GetAll");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var projectsList = JsonConvert.DeserializeObject<List<Project>>(jsonString);
                return View(projectsList);
            }

            return View(new List<Project>());
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
                _projectService.Insert(Proj);
                return RedirectToAction("Index");
            }
            return View();
        }
        
        //Update using Stored Procedure, its implementation is in ProjectService.cs
        public IActionResult Update(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var Proj = _projectService.GetById(id.Value);
            return View(Proj);
        }
        //Update using Stored Procedure
        [HttpPost]
        public IActionResult Update(Project Proj)
        {
            if (ModelState.IsValid)
            {
                _projectService.Update(Proj);
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
            var Proj = _projectService.GetById(id.Value);
            return View(Proj);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _projectService.Delete(id.Value);
            return RedirectToAction("Index");
        }


    }
}
