namespace RaceDay.MVC.Models
{
    public class OrganiserEnrolmentViewModel
    {
        public int EnrolmentID { get; set; }
        public int ParticipantID { get; set; }
        public int EventID { get; set; }
        public int CategoryID { get; set; }
        public DateTime EnrolmentDate { get; set; }
        public string EnrolmentStatus { get; set; } = "";
    }
}