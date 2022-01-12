using Microsoft.AspNetCore.Mvc;

namespace Mediscreen.Controllers
{
    public class MainController : Controller
    {
        [HttpGet("/")]
        public IActionResult Home() => View();
    }
}