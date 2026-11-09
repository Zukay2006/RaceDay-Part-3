namespace RaceDay.MVC.Models
{
    public class ProfileViewModel
    {
        public int ParticipantID { get; set; }

        public int UserId { get; set; }

        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Role { get; set; } = "";

        public string? ProfilePictureUrl { get; set; }

        public string? NewPassword { get; set; }
    }
}