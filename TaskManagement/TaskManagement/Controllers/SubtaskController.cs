using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Services;

namespace TaskManagement.Controllers
{
    [Authorize]
    public class SubtaskController : Controller
    {
        private readonly ISubtaskService _iSubtaskService;

        public SubtaskController(ISubtaskService iSubtaskService)
        {
            _iSubtaskService = iSubtaskService;
        }
        public IActionResult Index()
        {
            var subtasksList = _iSubtaskService.GetAll();
            return View(subtasksList);
        }
    }
}
