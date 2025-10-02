using Microsoft.EntityFrameworkCore;
using TrainMonitor.Data;
using TrainMonitor.Interfaces;
using TrainMonitor.Models;

namespace TrainMonitor.Repositories
{
    public class TrainRepository : ITrainRepository
    {
        private readonly AppDbContext _context;

        public TrainRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Train>> GetAll()
            => await _context.Trains.Include(t => t.Incidents).AsNoTracking().ToListAsync();

        public async Task<Train> GetById(Guid id)
            => await _context.Trains.Include(t => t.Incidents).FirstOrDefaultAsync(t => t.Id == id);
    }
}
