using Microsoft.EntityFrameworkCore;
using TrainMonitor.Data;
using TrainMonitor.Models;
using TrainMonitor.Repositories;

namespace TrainMonitor.Tests.ServicesTests
{
    public class TrainRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly TrainRepository _repo;

        public TrainRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repo = new TrainRepository(_context);
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllTrains()
        {
            var train1 = new Train { Id = Guid.NewGuid(), Name = "Test Train 1", Number = "001", DelayMinutes = 2, NextStop = "Chisinau" };
            var train2 = new Train { Id = Guid.NewGuid(), Name = "Test Train 2", Number = "002", DelayMinutes = 0, NextStop = "Tiraspol" };
            _context.Trains.Add(train1);
            _context.Trains.Add(train2);
            await _context.SaveChangesAsync();

            var trains = await _repo.GetAll();

            Assert.NotNull(trains);
            Assert.Equal(2, trains.Count());
        }

        [Fact]
        public async Task GetAll_ShouldReturnZeroTrains()
        {
            var trains = await _repo.GetAll();

            Assert.NotNull(trains);
            Assert.Equal(0, trains.Count());
        }

        [Fact]
        public async Task GetById_ShouldReturnCorrectTrain()
        {
            var train = new Train { Id = Guid.NewGuid(), Name = "Test Train", Number = "001", DelayMinutes = 0, NextStop = "Chisinau" };

            _context.Trains.Add(train);
            await _context.SaveChangesAsync();

            var newTrain = await _repo.GetById(train.Id);

            Assert.NotNull(newTrain);
            Assert.Equal("Test Train", newTrain.Name);
        }

        [Fact]
        public async Task GetById_ShouldReturnNull()
        {
            var trainId = Guid.NewGuid();
            var newTrain = await _repo.GetById(trainId);

            Assert.Null(newTrain);
        }
    }
}
