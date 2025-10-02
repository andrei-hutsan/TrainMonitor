using Microsoft.AspNetCore.Mvc;
using Moq;
using TrainMonitor.Controllers;
using TrainMonitor.Interfaces;
using TrainMonitor.Models;

namespace TrainMonitor.Tests.ControllerTests
{
    public class TrainControllerTests
    {
        private readonly Mock<ITrainRepository> _mockRepo;
        private readonly TrainController _controller;
        public TrainControllerTests()
        {
            _mockRepo = new Mock<ITrainRepository>();
            _controller = new TrainController(_mockRepo.Object);
        }

        [Fact]
        public async Task Index_ShouldReturnViewWithTrains()
        {
            var trains = new List<Train>
            {
                new Train { Id = Guid.NewGuid(), Name = "Train 1", Number = "T1" },
                new Train { Id = Guid.NewGuid(), Name = "Train 2", Number = "T2" }
            };

            _mockRepo.Setup(r => r.GetAll()).ReturnsAsync(trains);

            var result = await _controller.Index() as ViewResult;

            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Train>>(result.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task Details_ShouldReturnNotFound_WhenTrainMissing()
        {
            _mockRepo.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync((Train)null);

            var result = await _controller.Details(Guid.NewGuid());

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ShouldReturnTrain_WhenFound()
        {
            var train = new Train { Id = Guid.NewGuid(), Name = "Express", Number = "E100" };

            _mockRepo.Setup(r => r.GetById(train.Id)).ReturnsAsync(train);

            var result = await _controller.Details(train.Id) as ViewResult;

            Assert.NotNull(result);
            var model = Assert.IsType<Train>(result.Model);
            Assert.Equal("Express", model.Name);
        }
    }
}
