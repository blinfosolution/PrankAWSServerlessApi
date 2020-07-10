using System;
using System.ComponentModel.DataAnnotations;
namespace PrankAWSWebApp.Areas.Admin.Models
{
    public class ReferralInfoAddEditModel
    {
        public int ReferralId { get; set; }

        public string ReferralTitle { get; set; }
        public string ReferralDesc { get; set; }
        public string ReferralCode { get; set; }
        [RegularExpression("^[0-9]*$", ErrorMessage = "Free Credits Point must be numeric")]
        [Required(ErrorMessage = "Please enter Free Credits Point.")]
        public int FreeCreditPoint { get; set; }
        public DateTime AddedDate { get; set; }
        public bool IsActive { get; set; }
   
    }
}
