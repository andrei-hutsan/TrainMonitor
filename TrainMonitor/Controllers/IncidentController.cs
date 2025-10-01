using Microsoft.AspNetCore.Mvc;
using TrainMonitor.Data;
using TrainMonitor.Models;

namespace TrainMonitor.Controllers
{
    public class IncidentController : Controller
    {

        private readonly AppDbContext _context;

        public IncidentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Report(Guid trainId)
        {
            ViewBag.TrainId = trainId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Report(Guid trainId, Incident incident)
        {
            if (!ModelState.IsValid)
                return View(incident);

            incident.TrainId = trainId;
            incident.Timestamp = DateTime.Now;

            _context.Incidents.Add(incident);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Train", new { id = trainId });
        }
    }
}
