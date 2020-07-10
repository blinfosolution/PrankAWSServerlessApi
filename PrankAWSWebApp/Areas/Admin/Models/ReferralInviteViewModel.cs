using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrankAWSWebApp.Areas.Admin.Models
{
    public class ReferralInviteViewModel
    {
        public int DeviceId { get; set; }
        public List<SelectListItem> DeviceKeyLst { get; set; }
    }
}
