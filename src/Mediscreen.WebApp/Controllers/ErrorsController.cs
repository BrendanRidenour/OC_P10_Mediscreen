using Mediscreen.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Mediscreen.Controllers
{
    [Route("[controller]")]
    public class ErrorsController : Controller
    {
        [HttpGet(nameof(ServerError))]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult ServerError()
        {
            return View(viewName: nameof(ServerError),
                model: new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet(nameof(StatusCodeError))]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult StatusCodeError([FromQuery] int code)
        {
            if (code == 404)
                return View("NotFound");

            return ServerError();
        }
    }
}