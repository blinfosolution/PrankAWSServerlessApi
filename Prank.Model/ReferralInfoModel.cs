using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prank.Model
{
    public class ReferralInfoModel
    {
        public int ReferralId { get; set; }
        public string ReferralTitle { get; set; }
        public string ReferralCode { get; set; }
        public string Description { get; set; }
        public int FreeCreditPoint { get; set; }
        //public DateTime AddedDate { get; set; }
        public bool IsActive { get; set; }
     
    }

    public class ReferralInfoListModel
    {
        public int ReferralId { get; set; }
        public string ReferralTitle { get; set; }
        public string ReferralCode { get; set; }
        public string Description { get; set; }
        public int FreeCreditPoint { get; set; }
        public DateTime AddedDate { get; set; }
        public bool IsActive { get; set; }
        public int TotalCount { get; set; }
    }
}
