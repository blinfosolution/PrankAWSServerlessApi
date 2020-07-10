using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Prank.Model
{
    public class DeviceInfoModel
    {
        public int DeviceId { get; set; }
        public string DeviceKey { get; set; }
        public string DeviceInfomation { get; set; }
        public bool IsActive { get; set; }
        [NotMapped]
        public string AddedDate { get; set; }
        [NotMapped]
        public string ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class DeviceInfoListModel
    {
        public int DeviceId { get; set; }
        public string DeviceKey { get; set; }
        public string DeviceInfomation { get; set; }
        public bool IsActive { get; set; }
       
        public DateTime AddedDate { get; set; }
     
        public DateTime? ModifiedDate { get; set; }
        public int? CreditBalance  { get; set; }
        public int TotalCount { get; set; }
    }
}
