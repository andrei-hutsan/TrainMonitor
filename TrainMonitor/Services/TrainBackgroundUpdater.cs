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
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }

        private async Task UpdateTrainsAsync()
        {
            try
            {
                var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data/task-j.json");

                if (!File.Exists(jsonPath))
                {
                    _logger.LogWarning("JSON file not found at {Path}", jsonPath);
                    return;
                }

                var jsonData = await File.ReadAllTextAsync(jsonPath);
                var root = JsonConvert.DeserializeObject<JsonRoot>(jsonData);

                if (root?.Data == null)
                {
                    _logger.LogWarning("No train data found in JSON");
                    return;
                }

                using var scope = _services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

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
                            NextStop = trainData.ReturnValue.NextStopObj?.Title,
                            LastUpdated = trainData.ReturnValue.UpdaterTimeStamp
                        });
                    }
                    else
                    {
                        train.DelayMinutes = trainData.ReturnValue.ArrivingTime;
                        train.NextStop = trainData.ReturnValue.NextStopObj?.Title;
                        train.LastUpdated = trainData.ReturnValue.UpdaterTimeStamp;
                    }
                }

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating trains from JSON.");
            }
        }
    }
}
