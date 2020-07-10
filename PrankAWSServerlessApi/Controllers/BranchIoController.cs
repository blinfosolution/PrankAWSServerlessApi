using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Prank.DAL;
using Prank.Model;
using PrankAWSServerlessApi.ApiSettings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using System;
using System.Net.Mail;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using PrankAWSServerlessApi.Common;
using PrankAWSServerlessApi.Common.BranchIo;

namespace PrankAWSServerlessApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchIoController : BaseApiController
    {
        public BranchIoController(DataContext dataContext, IConfiguration config, FileSettings fileSettings, IWebHostEnvironment env) : base(dataContext, config, fileSettings, env)
        {
        }

        [HttpGet]
        [Route("SendBranchIoLinkEmail")]
        public async Task<ActionResult<ResponseModel>> SendBranchIoLinkEmail(string emailTo)
        {
            var model = new ResponseModel();

            string subject = "Branch io test";
            string body = "Email body ";
            string FromMail = _IConfiguration["EmailSettings:SmtpFrom"].ToString();// "sharmadiw@gmail.com";
            //string emailTo = "sharmadiw@gmail.com";

            var branchKey = _IConfiguration["BranchIo:branchkey"].ToString();
            var url = "https://www.trydagga.com";
            string branchIoUrl = await GetUrlData(url, branchKey);
            var urlValue = JsonConvert.DeserializeObject<Dictionary<string, string>>(branchIoUrl);

            string _samplestring = "You also have to click on is <a href='{0}' target='_blank'> link </a>";
            string _url = urlValue.FirstOrDefault().Value;

            body += string.Format(_samplestring, _url);
            var smtp = new SmtpClient
            {
                Host = _IConfiguration["EmailSettings:SmtpServer"].ToString(),// "smtp.ipower.com",
                Port = Convert.ToInt32(_IConfiguration["EmailSettings:SmtpPort"]),// 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                //Credentials = new networkcredential("customerservice@myclaimslawyer.com", "ClaimLawyer@123")
                Credentials = new NetworkCredential(_IConfiguration["EmailSettings:SmtpUserName"].ToString(), _IConfiguration["EmailSettings:SmtpPassword"].ToString())
            };
            using (var message = new MailMessage(FromMail, emailTo)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                try
                {
                    smtp.Send(message);
                    model.ResponseObject = "Link send susscessfully";
                    model.StatusCode = 200;
                }
                catch (Exception ex)
                {
                    model.ResponseObject = "Link not sent";
                    model.StatusCode = 404;
                }
            return model;
        }

        [HttpGet]
        [Route("GetBranchIoUrl")]
        public async Task<ActionResult<ResponseModel>> GetBranchIoUrl()
        {
            var branchKey = _IConfiguration["BranchIo:branchkey"].ToString();
            var url = "https://www.trydagga.com";

            string branchIoUrl = await GetUrlData(url, branchKey);
            var model = new ResponseModel();
            if (branchIoUrl != null)
            {
                model.ResponseObject = branchIoUrl;
                model.StatusCode = 200;
            }
            else
            {
                model.StatusCode = 404;
            }
            return model;
        }

        [HttpGet]
        [Route("GetUrlData")]
        public async Task<string> GetUrlData(string url, string branchKey)
        {

            const string BranchIOUrl = "https://api.branch.io/v1/url";
            string responseContent = string.Empty;
            HttpClient client = new HttpClient();

            //client.BaseAddress = new Uri("https://api.branch.io");
            client.BaseAddress = new Uri("https://api.branch.io/v1/url");
            client.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var payload = new Rootobject();
            payload.branch_key = branchKey;
            payload.channel = "mobile_web";
            payload.feature = "create_link";


            var stringPayload = JsonConvert.SerializeObject(payload);
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(BranchIOUrl, httpContent);

            if (response.Content != null)
            {
                responseContent = await response.Content.ReadAsStringAsync();
            }
            return responseContent;
        }

       

       
    }
}