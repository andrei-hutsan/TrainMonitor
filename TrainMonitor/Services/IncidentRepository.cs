using TrainMonitor.Data;
using TrainMonitor.Interfaces;
using TrainMonitor.Models;

namespace TrainMonitor.Services
{
    public class IncidentRepository : IIncidentRepository
    {
        private readonly AppDbContext _context;
        public IncidentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddReport(Incident entity)
        {
            _context.Incidents.Add(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
