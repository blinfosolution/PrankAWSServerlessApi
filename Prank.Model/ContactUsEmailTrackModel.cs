using System;
using System.Collections.Generic;
using System.Text;

namespace Prank.Model
{
    public class ContactUsEmailTrackModel
    {
        public int EmailTrackId { get; set; }
        public string EmailTo { get; set; }
        public string Messages { get; set; }
        public DateTime SendDate { get; set; }
        public int TotalCount { get; set; }
    }
}
