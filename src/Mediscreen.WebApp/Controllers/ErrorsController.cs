using Microsoft.AspNetCore.Mvc;

namespace Mediscreen.Controllers
{
    [Route("[controller]")]
    public class ErrorsController : Controller
    {
        [HttpGet(nameof(ServerError))]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult ServerError([FromQuery] int? statusCode)
        {
            if (statusCode == 404)
                return View("NotFound");

            return View();
        }
    }
}