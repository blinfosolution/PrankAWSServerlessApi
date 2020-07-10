using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrankAWSWebApp.ApiSettings 
{
    public class TwilioSettings
    {
        public string TwilioAccountSid { get; set; }
        public string TwilioAuthToken { get; set; }
        public string TwilioApiKey { get; set; }
        public string TwilioApiSecret { get; set; }
        public string OutgoingApplicationSid { get; set; }
        public string TwilioIdentity { get; set; }
        public string TwilioApiUrl {get;set;}
    }
}
