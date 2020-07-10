using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrankAWSWebApp.ApiSettings
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; }
        public string SmtpUserName { get; set; }
        public string SmtpPassword { get; set; }
        public string SmtpFrom { get; set; }
        public int SmtpPort { get; set; }
        public bool EmailsInTestMode { get; set; }
        public string EmailToTest { get; set; }
        public string EmailBcc { get; set; }
        public string EmailTo { get; set; }
    }
}
