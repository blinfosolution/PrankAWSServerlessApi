using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrankAWSWebApp.Models
{
    public class PackagesModelViewModel
    {
        public int PackageId { get; set; }
        [Required(ErrorMessage = "Please enter title. ")]
        public string PackageTitle { get; set; }

        [RegularExpression(@"^(0|-?\d{0,16}(\.\d{0,2})?)$", ErrorMessage = "Package price must be numeric")]
        [Required(ErrorMessage = "Please enter Package Price.")]
        public decimal PackagePrice { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Token Credits Point must be numeric")]
        [Required(ErrorMessage = "Please enter Token Credits Point.")]
        public int TokenCreditsPoint { get; set; }
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Please enter sort order.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Sort Order must be numeric")]
        public int SortOrder { get; set; }

        [Required(ErrorMessage = "Please enter ProductIdentifier.")]
        public string ProductIdentifier { get; set; }
        public string AddedDate { get; set; }
        public string ModifiedDate { get; set; }
        public int TotalCount { get; set; }
    }
    public class PackagesDataModel
    {
        public string statusCode { get; set; }
        public List<PackagesModelViewModel> responseObject { get; set; }
        public string message { get; set; }
    }
}
