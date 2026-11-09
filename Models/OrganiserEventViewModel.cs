using System.ComponentModel.DataAnnotations;

namespace RaceDay.MVC.Models
{
    public class OrganiserEventViewModel
    {
        public int EventID { get; set; }

        [Required]
        [StringLength(100)]
        public string EventName { get; set; } = "";

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        [StringLength(150)]
        public string Location { get; set; } = "";

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [StringLength(50)]
        public string EventType { get; set; } = "";

        [Range(0.01, 9999)]
        public decimal DistanceKM { get; set; }

        public string Status { get; set; } = "Upcoming";
    }
}