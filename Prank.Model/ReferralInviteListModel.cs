using System;
using System.Collections.Generic;
using System.Text;

namespace Prank.Model
{
   public class ReferralInviteListModel
    {
        public int ReferralInviteId { get; set; }
        public int FromDeviceId { get; set; }
        public int ToDeviceId { get; set; }
        public int FreeToken { get; set; }     
        public string Status { get; set; }
        public DateTime ReferDate { get; set; }
        public DateTime? ReferAcceptedDate { get; set;}
        public string FromDevice { get; set; }
        public string ToDevice { get; set; }
        public int TotalCount { get; set; }
    }

    public class ReferralInviteInfoModel
    {
        public int ReferralInviteId { get; set; }
        public int FromDeviceId { get; set; }
        public int ToDeviceId { get; set; }
        public int PackageOderId { get; set; }
        public string Status { get; set; }
        public int FreeToken { get; set; }
        public DateTime ReferDate { get; set; }
        public DateTime ReferAcceptedDate { get; set; }

        public string ReferralCode { get; set; }
        public string ReferralLink { get; set; }
        public string ReferralLinkKey { get; set; }

    }
}
