namespace TrainMonitor.Models
{
    public class Train
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public int? DelayMinutes { get; set; }
        public string NextStop { get; set; }
        public DateTime LastUpdated { get; set; }


        public ICollection<Incident> Incidents { get; set; }
    }
}
