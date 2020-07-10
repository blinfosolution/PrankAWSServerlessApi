using System.ComponentModel.DataAnnotations;

namespace PrankAWSWebApp.Areas.Admin.Models
{
    public class PrankCallFromPhoneNumberAddEditModel
    {
        public int PrankCallFromId { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string CountryCode { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }

    }
}

