using Microsoft.AspNetCore.Mvc;
using Xunit;
using static ThoughtHaven.TestHelpers;

namespace Mediscreen.Controllers
{
    public class ErrorsControllerTests
    {
        [Fact]
        public void InheritsController()
        {
            Assert.True(typeof(Controller).IsAssignableFrom(typeof(ErrorsController)));
        }

        [Fact]
        public void HasRouteAttribute()
        {
            var attribute = GetClassAttribute<ErrorsController, RouteAttribute>();

            Assert.Equal("[controller]", attribute.Template);
        }

        [Fact]
        public void ServerError_HasHttpGetAttribute()
        {
            var attribute = GetMethodAttribute<ErrorsController, HttpGetAttribute>("ServerError");

            Assert.Equal("ServerError", attribute.Template);
        }

        [Fact]
        public void ServerError_HasResponseCacheAttribute()
        {
            var attribute = GetMethodAttribute<ErrorsController, ResponseCacheAttribute>("ServerError");

            Assert.Equal(0, attribute.Duration);
            Assert.Equal(ResponseCacheLocation.None, attribute.Location);
            Assert.True(attribute.NoStore);
        }

        [Fact]
        public void CreatePatient_NullStatusCode_ReturnsView()
        {
            var controller = Controller();

            var result = controller.ServerError(statusCode: null);

            var view = Assert.IsType<ViewResult>(result);
            Assert.Null(view.ViewName);
        }

        [Theory]
        [InlineData(400)]
        [InlineData(500)]
        public void CreatePatient_StatusCodeEqualsNot404_ReturnsView(int statusCode)
        {
            var controller = Controller();

            var result = controller.ServerError(statusCode);

            var view = Assert.IsType<ViewResult>(result);
            Assert.Null(view.ViewName);
        }

        [Fact]
        public void CreatePatient_StatusCode404_ReturnsView()
        {
            var controller = Controller();

            var result = controller.ServerError(statusCode: 404);

            var view = Assert.IsType<ViewResult>(result);
            Assert.Equal("NotFound", view.ViewName);
        }

        static ErrorsController Controller() => new();
    }
}