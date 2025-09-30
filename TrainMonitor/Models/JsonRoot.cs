namespace TrainMonitor.Models
{
    public class JsonRoot
    {
        public string Type { get; set; }
        public List<TrainData> Data { get; set; }
    }
}
