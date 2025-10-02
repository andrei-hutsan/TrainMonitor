using Microsoft.EntityFrameworkCore;
using TrainMonitor.Data;
using TrainMonitor.Models;
using TrainMonitor.Repositories;

namespace TrainMonitor.Tests.ServicesTests
{
    public class IncidentRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly IncidentRepository _repo;

        public IncidentRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repo = new IncidentRepository(_context);
        }

        [Fact]
        public async Task AddReport_ShouldSaveIncident()
        {
            var incident = new Incident
            {
                Id = Guid.NewGuid(),
                TrainId = Guid.NewGuid(),
                Username = "Andrei",
                Reason = "Big Delay",
                Comment = "Over 30 minutes late",
                Timestamp = DateTime.UtcNow
            };

            var result = await _repo.AddReport(incident);

            Assert.True(result);
            Assert.Single(_context.Incidents);
            Assert.Equal("Andrei", _context.Incidents.First().Username);
        }
    }
}
