using System;

namespace Prank.Model
{
    public   class TrackingInfoModel
    {
        public string EmployeeId { get; set; }
        public string ModuleName { get; set; }
        public string WorkDescription { get; set; }
       
    }
    public class TrackingInfoListModel
    {
        public int TrackingId { get; set; }        
        public string FirstName  { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string ModuleName { get; set; }
        public string WorkDescription { get; set; }
        public DateTime TrackDate { get; set; }
        public int TotalCount { get; set; }

    }
}
