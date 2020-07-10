using System;
using System.Collections.Generic;
using System.Text;

namespace Prank.Model
{
    public class EmailMessage
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string FromName { get; set; }
        public List<string> CC { get; set; }
        public List<string> BCC { get; set; }
        public List<string> FileAttachments { get; set; }
        public List<MessageAttachment> Attachments { get; set; }

        public string CCString => CC != null ? string.Join(",", CC) : null;
        public string BCCString => BCC != null ? string.Join(",", BCC) : null;
    }
    public class MessageAttachment
    {
        public string Name { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
    }
}
