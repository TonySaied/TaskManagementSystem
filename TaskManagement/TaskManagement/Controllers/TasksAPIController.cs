using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Models;

namespace TaskManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksAPIController : ControllerBase
    {
        ApplicationDbContext _context;
        public TasksAPIController(ApplicationDbContext context)
        {
            _context = context;
        }
        //https://localhost:7124/api/TasksApi/Overdue?count=2 => Testing Link
        [HttpGet("Overdue")]
        public IActionResult GetOverdueTasks(int count)
        {
            var overdueTasks = _context.Tasks
                .Where(t => t.DueDate < DateTime.Now).Take(count).ToList();

            return Ok(overdueTasks);
        }
    }
}
