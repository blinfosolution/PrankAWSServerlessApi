using System;

namespace Prank.Model
{
    public class PrankCallFromPhoneNumberModel
    {
        public int PrankCallFromId { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        //public bool IsDeleted { get; set; }

        //[NotMapped]
        //public DateTime AddedDate { get; set; }

        //[NotMapped]
        //public DateTime ModifiedDate { get; set; }
    }

    public class PrankCallFromPhoneNumberLstModel
    {
        public int PrankCallFromId { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
        public DateTime AddedDate { get; set; }
        public int TotalCount { get; set; }
    }
}
