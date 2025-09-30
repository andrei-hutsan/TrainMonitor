namespace TrainMonitor.Models
{
    public class ReturnValue
    {
        public string Train { get; set; }
        public int? ArrivingTime { get; set; }
        public Station NextStopObj { get; set; }
        public DateTime UpdaterTimeStamp { get; set; }
    }
}
