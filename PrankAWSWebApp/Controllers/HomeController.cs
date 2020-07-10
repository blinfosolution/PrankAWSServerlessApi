using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Prank.Model;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML;
using Twilio.TwiML.Voice;

namespace PrankAWSWebApp.Controllers
{
    public class HomeController : Controller
    {
       
        public ActionResult ConfrenceCallee(string confrenceFriendlyName)
        {
            string TwilioApiUrl = "http://5d30045dd6b7.ngrok.io/Home/";
            var model = new ResponseModel();
            var noOfParticipants = HttpContext.Request.Form["SequenceNumber"];
            int participants = 0;

            int.TryParse(noOfParticipants, out participants);
            if (HttpContext.Request.Form["StatusCallbackEvent"] == "participant-join" && participants > 1)
            {
                TwilioClient.Init("ACcdbc21ca5ba9fd8ef16087e57fe64cd4", "b9cbe4da5f9760e8ccfccd6c53edd45a");

                var conferences = ConferenceResource.Read(
                    friendlyName: confrenceFriendlyName,
                    status: ConferenceResource.StatusEnum.InProgress,
                    limit: 20
                );

                string pathConferenceSid = string.Empty;
                foreach (var record in conferences)
                {
                    pathConferenceSid = record.Sid;
                }
                string joinConfrenceCallbackUrl = TwilioApiUrl + "JoinConfrenceCallback";
                var participant = ConferenceResource.Update(
                      announceUrl: new Uri(joinConfrenceCallbackUrl),
                      pathSid: pathConferenceSid


                );
                model.StatusCode = 200;




            }
            return Json(model);
        }
        public ActionResult JoinConfrenceCallback()
        {
            var response = new VoiceResponse();
            var play = new Play(url: new Uri("https://prankapibucket.s3.ap-south-1.amazonaws.com/Audio/d88e5d94-d8f8-478d-a92f-8cf4e21d5572.mp3"));
            response.Append(play);
            return Content(response.ToString());
        }

    }
}
