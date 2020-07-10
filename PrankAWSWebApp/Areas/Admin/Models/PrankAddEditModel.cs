using System;
using System.ComponentModel.DataAnnotations;
namespace PrankAWSWebApp.Areas.Admin.Models
{
    public class PrankAddEditModel
    {
        public int PrankId { get; set; }

        [Required(ErrorMessage = "Please enter prank name.")]
        public string PrankName { get; set; }

        [Required(ErrorMessage = "Please enter prank desc.")]
        public string PrankDesc { get; set; }

        [Required(ErrorMessage = "Please enter prank image.")]
        public string PrankImage { get; set; }

        [Required(ErrorMessage = "Please enter prank audio file.")]
        public string PreviewAudioFile { get; set; }

        [Required(ErrorMessage = "Please enter main prank audio file.")]
        public string MainAudioFile { get; set; }

        public string PlivoAudioXmlUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
