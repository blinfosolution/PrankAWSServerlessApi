using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prank.Model
{
    public class PrankCallRequestModel
    { 
        public int PrankId { get; set; }
        public int DeviceId { get; set; }
        public string ToPhoneNumberPersonName { get; set; }
        public string ToPhoneNumberCountryCodePrefix { get; set; }
        public string ToPhoneNumber { get; set; }
        public bool IsRecordCall { get; set; }
        public int CostPoint { get; set; }
        [NotMapped]
        public int PrankCallFromId { get; set; }
    }
    public class PrankCallTrackingModel
    {
        public int TrackingId { get; set; }
        public int DeviceId { get; set; }
        public int PrankId { get; set; }
        public int PrankCallPoints { get; set; }
        public int PrankCallFromId { get; set; }
        public string ToPhoneNumberPersonName { get; set; }
        public string ToPhoneNumber { get; set; }
        public bool IsSavePhoneCall { get; set; }
        public string PlivoRecordCallUrl { get; set; }
        public string RecordedAudioFile { get; set; }
        public string AddedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class PrankCallHistoryModel
    {
        public int TrackingId { get; set; }
        public string PrankName { get; set; }
        public string PrankImage { get; set; }
        public string ToPhoneNumberPersonName { get; set; }
        public string ToPhoneNumber { get; set; }
        public bool IsSavePhoneCall { get; set; }
        public string PlivoRecordCallUrl { get; set; }
        public string RecordedAudioFile { get; set; }
        public DateTime CallDate { get; set; }
        public int PrankCallPoints { get; set; }
        public int TotalCount{get;set;}
    }
}
