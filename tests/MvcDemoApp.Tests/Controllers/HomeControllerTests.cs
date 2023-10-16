using Xunit;
using Microsoft.AspNetCore.Mvc;
using MvcDemoApp.Controllers;
using Microsoft.Extensions.Logging;
using Moq;

namespace MvcDemoApp.Tests
{
    public class HomeControllerTests
    {
        private readonly Mock<ILogger<HomeController>> _mockLogger = new Mock<ILogger<HomeController>>();
        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            _controller = new HomeController(_mockLogger.Object);
        }

        [Fact]
        public void Index_ReturnsViewResult()
        {
            var result = _controller.Index();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Privacy_ReturnsViewResult()
        {
            var result = _controller.Privacy();
            
            Assert.IsType<ViewResult>(result);
        }
    }
}