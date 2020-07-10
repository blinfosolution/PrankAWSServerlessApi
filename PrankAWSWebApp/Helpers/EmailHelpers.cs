using Prank.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace PrankAWSWebApp.Helpers
{
    public class EmailHelpers
    {
        public static Task SendAsync(string subject, string messageBody, string emailTo,
           string smtpServer, int smtpPort, string smtpUserName, string smtpPassword, string smtpFrom,
           bool isHtml = true,
           List<string> cc = null, List<string> bcc = null,
           List<string> attachments = null, List<MessageAttachment> attachmentsData = null)
        {


            // Configure the client:
            var client =
                new SmtpClient(smtpServer)
                {
                    Port = smtpPort,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false
                };

            // Create the credentials:
            var credentials =
                new NetworkCredential(smtpUserName, smtpPassword);

            client.EnableSsl = true;
            client.Credentials = credentials;

            // Create the message:
            var mail =
                new MailMessage(smtpFrom, emailTo)
                {
                    Subject = subject,
                    Body = messageBody,
                    IsBodyHtml = isHtml
                };
            if (cc != null)
            {
                foreach (var c in cc)
                {
                    mail.CC.Add(c);
                }
            }
            if (bcc != null)
            {
                foreach (var c in bcc)
                {
                    mail.Bcc.Add(c);
                }
            }

            if (attachments != null)
            {
                foreach (var file in attachments)
                {
                    Attachment data = new Attachment(file, MediaTypeNames.Application.Octet);
                    // Add time stamp information for the file.
                    ContentDisposition disposition = data.ContentDisposition;
                    disposition.CreationDate = System.IO.File.GetCreationTime(file);
                    disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
                    disposition.ReadDate = System.IO.File.GetLastAccessTime(file);
                    // Add the file attachment to this e-mail message.
                    mail.Attachments.Add(data);
                }
            }

            if (attachmentsData != null)
            {
                foreach (var file in attachmentsData)
                {
                    if (file.Data != null)
                    {
                        MemoryStream stream = new MemoryStream(file.Data, false);
                        //stream.Write(file.Data, 0, file.Data.Length);
                        Attachment data = new Attachment(stream, file.Name, file.ContentType);
                        mail.Attachments.Add(data);
                    }
                }
            }

            client.EnableSsl = true;

            // Send:
            return client.SendMailAsync(mail);
        }
    }
}
