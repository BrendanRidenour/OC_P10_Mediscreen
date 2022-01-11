using Microsoft.AspNetCore.Mvc;

namespace Mediscreen.WebApp.Controllers
{
    public class MainController : Controller
    {
        [HttpGet("/")]
        public IActionResult Home()
        {
            return View();
        }
    }
}