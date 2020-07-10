using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prank.Model
{
    public class PrankInfoModel
    {
        public int PrankId { get; set; }
        public string PrankName { get; set; }
        public string PrankDesc { get; set; }
        public string PrankImage { get; set; }
        public string PreviewAudioFile { get; set; }
        public string MainAudioFile { get; set; }
        public bool IsActive { get; set; }
        public string PlivoAudioXmlUrl { get; set; }

    }
    public class PrankInfoListModel
    {
        public int PrankId { get; set; }
        public string PrankName { get; set; }
        public string PrankDesc { get; set; }
        public string PrankImage { get; set; }
        public string PreviewAudioFile { get; set; }
        public string MainAudioFile { get; set; }

        public bool IsActive { get; set; }
        //  [NotMapped]
        public DateTime AddedDate { get; set; }
       
        public DateTime? ModifiedDate { get; set; }
        public string PlivoAudioXmlUrl { get; set; }
        public int TotalCount { get; set; }
    }
}
