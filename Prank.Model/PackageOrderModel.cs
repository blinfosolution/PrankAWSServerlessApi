using System;
using System.Collections.Generic;
using System.Text;

namespace Prank.Model
{
    public class PackageOrderModel
    {
        public int PackageOrderId { get; set; }
        public int DeviceId { get; set; }
        public int PackageId { get; set; }
        public string TransactionId { get; set; }
        public string CustomerEmail { get; set; }
        public string AppItemId { get; set; }
        public string OrderStatus { get; set; }
        public string OrderStatusDescription { get; set; }
        public string PackageTitle { get; set; }
        public decimal PaidPrice { get; set; }
        public int TokenCreditPoint { get; set; }
        public string TokenEarnType { get; set; }
        public DateTime OrderDate  { get; set; }
        
    }

 
}
