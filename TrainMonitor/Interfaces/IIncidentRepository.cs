using TrainMonitor.Models;

namespace TrainMonitor.Interfaces
{
    public interface IIncidentRepository
    {
        Task<bool> AddReport(Incident entity);
    }
}