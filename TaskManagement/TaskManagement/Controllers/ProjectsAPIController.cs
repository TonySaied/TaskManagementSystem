using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Services;

namespace TaskManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsAPIController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsAPIController(IProjectService projectService)
        {
            _projectService = projectService;
        }
        //https://localhost:7124/api/ProjectsAPI/GetAll => Link
        [HttpGet("GetAll")]
        public IActionResult Index()
        {
            var ProjectsList = _projectService.GetAll();
            return Ok(ProjectsList);

        }
    }
}