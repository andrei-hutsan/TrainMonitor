using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainMonitor.Data;
using TrainMonitor.Interfaces;

namespace TrainMonitor.Controllers
{
    public class TrainController : Controller
    {
        private readonly ITrainRepository _trainRepository;

        public TrainController(ITrainRepository trainRepository)
        {
            _trainRepository = trainRepository;
        }

        public async Task<IActionResult> Index()
        {
            var trains = await _trainRepository.GetAll();
            return View(trains);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            if (id.Equals(null) || id == Guid.Empty)
                return BadRequest();

            var train = await _trainRepository.GetById(id);

            if (train == null)
                return NotFound();

            return View(train);
        }

        public async Task<IActionResult> TrainTable()
        {
            var trains = await _trainRepository.GetAll();
            return PartialView("_TrainTablePartial", trains);
        }

    }
}
