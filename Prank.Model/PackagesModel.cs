using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Prank.Model
{
    public class PackagesModel
    {
        public int PackageId { get; set; }
        public string PackageTitle { get; set; }
        public decimal PackagePrice { get; set; }
        public int TokenCreditsPoint { get; set; }
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }

        [NotMapped]
        public DateTime AddedDate { get; set; }

        [NotMapped]
        public DateTime ModifiedDate { get; set; }

       public string ProductIdentifier { get; set; }
        public int TotalCount { get; set; }
    }
}
