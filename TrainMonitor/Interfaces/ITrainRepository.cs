using TrainMonitor.Models;

namespace TrainMonitor.Interfaces
{
    public interface ITrainRepository
    {
        Task<IEnumerable<Train>> GetAll();
        Task<Train> GetById(Guid id);
    }
}