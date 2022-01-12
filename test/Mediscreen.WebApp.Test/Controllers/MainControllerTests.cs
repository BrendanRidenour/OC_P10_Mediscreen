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
        public void Home_ReturnsView()
        {
            var controller = new MainController();

            var result = controller.Home();

            var view = Assert.IsType<ViewResult>(result);
            Assert.Null(view.ViewName);
        }
    }
}