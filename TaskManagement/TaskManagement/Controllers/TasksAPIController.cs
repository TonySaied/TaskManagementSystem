using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using TaskManagement.Services;

namespace TaskManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksAPIController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksAPIController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        // https://localhost:7124/api/TasksApi/Overdue?count=2 => Testing Link
        [HttpGet("Overdue")]
        public IActionResult GetOverdueTasks(int count)
        {
            var overdueTasks = _taskService.GetAll()
                                            .Where(t => t.DueDate < DateTime.Now)
                                            .Take(count)
                                            .ToList();
            //NewtonsoftJson is used to avoid cyclic exception
            var jsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            return new JsonResult(overdueTasks, jsonSettings);
        }
    }
}
