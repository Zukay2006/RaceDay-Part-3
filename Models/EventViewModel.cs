
namespace RaceDay.MVC.Models
{
    public class EventViewModel
    {
        public int EventID { get; set; }

        public string EventName { get; set; } = "";

        public DateTime EventDate { get; set; }

        public string Location { get; set; } = "";

        public string? Description { get; set; }

        public string EventType { get; set; } = "";

        public decimal DistanceKM { get; set; }

        public string Status { get; set; } = "Upcoming";
    }
}
