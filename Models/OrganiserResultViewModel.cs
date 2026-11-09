namespace RaceDay.MVC.Models
{
    public class OrganiserResultViewModel
    {
        public int ResultID { get; set; }
        public int EnrolmentID { get; set; }
        public string EventName { get; set; } = "";
        public DateTime EventDate { get; set; }
        public string CategoryName { get; set; } = "";
        public TimeSpan? FinishTime { get; set; }
        public int? Position { get; set; }
        public int TotalFinishers { get; set; }
        public string ResultStatus { get; set; } = "Pending";
    }
}