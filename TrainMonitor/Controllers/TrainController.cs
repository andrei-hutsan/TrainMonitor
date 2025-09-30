using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainMonitor.Data;

namespace TrainMonitor.Controllers
{
    public class TrainController : Controller
    {
        private readonly AppDbContext _context;

        public TrainController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var trains = await _context.Trains
                .Include(t => t.Incidents)
                .ToListAsync();
            return View(trains);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var train = await _context.Trains
                .Include(t => t.Incidents)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (train == null) return NotFound();

            return View(train);
        }
    }
}
