using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Prank.DAL;
using Prank.Model;
using PrankAWSServerlessApi.ApiSettings;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace PrankAWSServerlessApi.Controllers
{
[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ContactUsEmailTrackController : BaseApiController
    {
        private readonly EmailSettings _emailSettings;
        public ContactUsEmailTrackController(DataContext dataContext, IConfiguration config, EmailSettings emailSettings, FileSettings fileSettings, IWebHostEnvironment env) : base(dataContext, config, fileSettings, env)
        {
            _emailSettings = emailSettings;
        }

        [HttpGet]
        [Route("GetContactUsEmailTrackByRequest")]
        public async Task<ActionResult<ResponseModel>> GetContactUsEmailTrackByRequest(string emailTo, string sortCol,string sortDir, int skip, int take)
        {
            emailTo = string.IsNullOrEmpty(emailTo) ? string.Empty : emailTo;
            var model = new ResponseModel();
            var objContactLst = await _dataContext.GetContactUsEmailTrackByRequest(emailTo,sortCol,sortDir,skip,take).ToListAsync();

            if (objContactLst != null)
            {
                model.ResponseObject = objContactLst;
                model.StatusCode = 200;
            }
            else
            {
                model.StatusCode = 404;
            }

            return model;
        }

        [HttpPost]
        [Route("AddContactUsRequest")]
        public async Task<ActionResult<ResponseModel>> AddContactUsRequest([FromBody]ContactUsEmailTrackModel emailTackinfo)
        {
            var model = new ResponseModel();
            var result = await _dataContext.AddContactUsRequest(emailTackinfo);

            if (!result.Status)
            {
                model.StatusCode = 404;
                model.Message = result.Message;
            }
            else
            {
                model.StatusCode = 200;

                //              #region Email Send 

                //              string emailTemplateFile = GetFilePath("ContactUsEmail", "");

                //              if (!System.IO.File.Exists(emailTemplateFile))
                //              {
                //                  throw new Exception("No email template found");
                //              }

                //              var mailBccEmail = new List<string>();
                //              string bccEmail = _emailSettings.EmailsInTestMode ? _emailSettings.EmailToTest : _emailSettings.EmailBcc;

                //              if (!string.IsNullOrEmpty(bccEmail))
                //              {
                //                  mailBccEmail.AddRange(bccEmail.Split(';'));
                //              }

                //              var message = new EmailMessage
                //              {
                //                  IsHtml = true,
                //                  Subject = "Prank Call",
                //                  To = _emailSettings.EmailsInTestMode ? _emailSettings.EmailToTest : emailTackinfo.EmailTo,
                //                  BCC = mailBccEmail,
                //              };

                //              var html = System.IO.File.ReadAllText(emailTemplateFile);

                //              message.Body = html.Replace("###Name###", emailTackinfo.EmailTo)
                //                                 .Replace("###Email###", emailTackinfo.EmailTo)
                //                                 .Replace("###Message###", emailTackinfo.Messages);

                //   await Helpers.EmailHelpers.SendAsync(message.Subject, message.Body, message.To, _emailSettings.SmtpServer,
                //                                       _emailSettings.SmtpPort, _emailSettings.SmtpUserName, _emailSettings.SmtpPassword,
                //                                       message.From ?? _emailSettings.SmtpFrom, message.IsHtml, message.CC,
                //                                       message.BCC);

                //              #endregion
            }

            return model;
        }
    }
}