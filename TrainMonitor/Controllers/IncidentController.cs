using Microsoft.AspNetCore.Mvc;
using TrainMonitor.Data;
using TrainMonitor.Interfaces;
using TrainMonitor.Models;

namespace TrainMonitor.Controllers
{
    public class IncidentController : Controller
    {
        private readonly IIncidentRepository _incidentRepository;

        public IncidentController(IIncidentRepository incidentRepository)
        {
            _incidentRepository = incidentRepository;
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

            var result = await _incidentRepository.AddReport(incident);
            if (!result)
                return View(incident);

            return RedirectToAction("Details", "Train", new { id = trainId });
        }
    }
}
