using System.ComponentModel.DataAnnotations;

namespace TrainMonitor.Models
{
    public class Incident
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Reason is required")]
        public string Reason { get; set; }

        [Required(ErrorMessage = "Comment is required")]
        public string Comment { get; set; }

        public Guid TrainId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

