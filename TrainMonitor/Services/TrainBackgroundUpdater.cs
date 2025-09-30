using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TrainMonitor.Data;
using TrainMonitor.Models;

namespace TrainMonitor.Services
{
    public class TrainBackgroundUpdater : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<TrainBackgroundUpdater> _logger;

        public TrainBackgroundUpdater(IServiceProvider services, ILogger<TrainBackgroundUpdater> logger)
        {
            _services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await UpdateTrainsAsync();
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        private async Task UpdateTrainsAsync()
        {
            try
            {
                var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data/task-j.json");

                if (!File.Exists(jsonPath))
                {
                    _logger.LogWarning("[Updater] JSON file not found at {Path}", jsonPath);
                    return;
                }

                var jsonData = await File.ReadAllTextAsync(jsonPath);
                var root = JsonConvert.DeserializeObject<JsonRoot>(jsonData);

                if (root?.Data == null || !root.Data.Any())
                {
                    _logger.LogWarning("[Updater] JSON parsed but contains no train data");
                    return;
                }

                using var scope = _services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                _logger.LogInformation("[Updater] Parsed {Count} trains from JSON", root.Data.Count);

                foreach (var trainData in root.Data)
                {
                    var trainNum = trainData.ReturnValue.Train;
                    var train = await context.Trains.FirstOrDefaultAsync(t => t.Number == trainNum);

                    if (train == null)
                    {
                        context.Trains.Add(new Train
                        {
                            Name = trainData.Name,
                            Number = trainData.ReturnValue.Train,
                            DelayMinutes = trainData.ReturnValue.ArrivingTime,
                            NextStop = trainData.ReturnValue.NextStopObj?.Title ?? "-",
                            LastUpdated = trainData.ReturnValue.UpdaterTimeStamp
                        });
                        _logger.LogInformation("[Updater] Inserted train: {Name} ({Number})", trainData.Name, trainNum);
                    }
                    else
                    {
                        train.DelayMinutes = trainData.ReturnValue.ArrivingTime;
                        train.NextStop = trainData.ReturnValue.NextStopObj?.Title ?? "-";
                        train.LastUpdated = trainData.ReturnValue.UpdaterTimeStamp;

                        _logger.LogInformation("[Updater] Updated train: {Name} ({Number})", trainData.Name, trainNum);
                    }
                }

                await context.SaveChangesAsync();
                _logger.LogInformation("[Updater] Database save completed at {Time}", DateTime.Now);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Updater] Error while updating trains from JSON.");
            }
        }
    }
}
