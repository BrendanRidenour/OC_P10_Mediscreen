using Microsoft.AspNetCore.Mvc;
using Xunit;
using static ThoughtHaven.TestHelpers;

namespace Mediscreen.Controllers
{
    public class MainControllerTests
    {
        [Fact]
        public void InheritsController()
        {
            Assert.True(typeof(Controller).IsAssignableFrom(typeof(MainController)));
        }

        [Fact]
        public void Home_HasHttpGetAttribute()
        {
            var attribute = GetMethodAttribute<MainController, HttpGetAttribute>("Home");

            Assert.Equal("/", attribute.Template);
        }

        [Fact]
        public void Home_ReturnsRedirectToAction()
        {
            var controller = new MainController();

            var result = controller.Home();

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("ReadPatients", redirect.ActionName);
            Assert.Equal("Patients", redirect.ControllerName);
        }
    }
}