
using System.ComponentModel.DataAnnotations;

namespace RaceDay.MVC.Models
{
    public class EnrolmentRequestViewModel
    {
        [Required]
        public int EventID { get; set; }

        [Required]
        public int CategoryID { get; set; }
    }
}
