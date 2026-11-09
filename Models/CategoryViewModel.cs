
namespace RaceDay.MVC.Models
{
    public class CategoryViewModel
    {
        public int CategoryID { get; set; }

        public int EventID { get; set; }

        public string CategoryName { get; set; } = "";

        public string ActivityType { get; set; } = "";

        public decimal DistanceKM { get; set; }

        public decimal EntryFee { get; set; }

        public string? EventName { get; set; }
    }
}
