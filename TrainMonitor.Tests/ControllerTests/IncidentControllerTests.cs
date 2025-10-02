using Microsoft.AspNetCore.Mvc;
using Moq;
using TrainMonitor.Controllers;
using TrainMonitor.Interfaces;
using TrainMonitor.Models;

namespace TrainMonitor.Tests.ControllerTests
{
    public class Incident_controllerTests
    {
        private readonly Mock<IIncidentRepository> _mockRepo;
        private readonly IncidentController _controller;
        public Incident_controllerTests()
        {
            _mockRepo = new Mock<IIncidentRepository>();
            _controller = new IncidentController(_mockRepo.Object);
        }


        [Fact]
        public void Report_Get_ShouldReturnViewWithTrainIdInViewBag()
        {
            var trainId = Guid.NewGuid();

            var result = _controller.Report(trainId) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(trainId, result.ViewData["TrainId"]);
        }

        [Fact]
        public async Task Report_Post_ShouldRedirect_WhenIncidentIsValid()
        {
            var trainId = Guid.NewGuid();
            var incident = new Incident
            {
                Username = "Andrei",
                Reason = "Technical issue",
                Comment = "Delay over 20 minutes"
            };

            _mockRepo.Setup(r => r.AddReport(It.IsAny<Incident>()))
                    .ReturnsAsync(true);

            var result = await _controller.Report(trainId, incident);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Details", redirect.ActionName);
            Assert.Equal("Train", redirect.ControllerName);
            Assert.Equal(trainId, redirect.RouteValues["id"]);
        }

        [Fact]
        public async Task Report_Post_ShouldReturnView_WhenModelStateIsInvalid()
        {
            var trainId = Guid.NewGuid();
            var incident = new Incident();

            _controller.ModelState.AddModelError("Username", "Required");

            var result = await _controller.Report(trainId, incident);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(incident, viewResult.Model);
        }

        [Fact]
        public async Task Report_Post_ShouldReturnView_WhenRepositoryFails()
        {
            var trainId = Guid.NewGuid();
            var incident = new Incident
            {
                Username = "Andrei",
                Reason = "Unknown issue",
                Comment = "Something went wrong"
            };

            _mockRepo.Setup(r => r.AddReport(It.IsAny<Incident>()))
                    .ReturnsAsync(false);

            var result = await _controller.Report(trainId, incident);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(incident, viewResult.Model);
        }
    }
}
