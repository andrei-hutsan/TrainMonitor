namespace TrainMonitor.Models
{
    public class Incident
    {
        public Guid Id { get; set; }
        public Guid TrainId { get; set; }
        public string Username { get; set; }
        public string Reason { get; set; }
        public string Comment { get; set; }
        public DateTime Timestamp { get; set; }


        public Train Train { get; set; }
    }
}
