using Prank.Model;
using PrankAWSServerlessApi.ApiSettings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Threading;
using Twilio.Jwt.AccessToken;
using Twilio;
using System.Text;
using Twilio.TwiML;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML.Voice;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Prank.DAL;
using Microsoft.Extensions.Hosting;

namespace PrankAWSServerlessApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwilioPrankController : BaseApiController
    {
        private readonly TwilioSettings _twilioSettings;
        private IWebHostEnvironment hostEnvironment;
        private string sessionID_to_callsid = string.Empty;

        string _host = string.Empty;
        public TwilioPrankController(DataContext dataContext, IConfiguration config, FileSettings fileSettings, TwilioSettings TwilioSettings, IWebHostEnvironment env) : base(dataContext, config, fileSettings, env)
        {
            _twilioSettings = TwilioSettings;
            hostEnvironment = env;
        }

        #region Twilio Call Conferance

        [HttpGet]
        [Route("TwilioAccessToken")]
        public ActionResult TwilioAccessToken()
        {
            var grant = new VoiceGrant();
            grant.OutgoingApplicationSid = _twilioSettings.OutgoingApplicationSid;

            // Optional: add to allow incoming calls
            grant.IncomingAllow = true;

            var grants = new HashSet<IGrant>
        {
            { grant }
        };

            // Create an Access Token generator
            var token = new Token(
                _twilioSettings.TwilioAccountSid,
               _twilioSettings.TwilioApiKey,
                _twilioSettings.TwilioApiSecret,
                 _twilioSettings.TwilioIdentity,
                grants: grants);

            return Content(token.ToJwt());
        }
        [HttpPost]
        [Route("MakeCall")]
        //    public async Task<ActionResult<ResponseModel>> CallByRequest(PrankCallRequestModel request)
        public async Task<ActionResult<ResponseModel>> CallByRequest()
        {
            int prankId = 4;
            int deviceId = 12;
            int prankCallFromId = 1;
            string toPhoneNumberCountryCodePrefix = "91";
            string ToPhoneNo = "7665704988";
            var request = new PrankCallRequestModel()
            {
                PrankId = prankId,
                DeviceId = deviceId,
                PrankCallFromId = prankCallFromId,
                ToPhoneNumberCountryCodePrefix = toPhoneNumberCountryCodePrefix,
                ToPhoneNumber = ToPhoneNo,
                CostPoint = 10,
                ToPhoneNumberPersonName = "sds"

            };

            var response = new VoiceResponse();
            var fromNumber = await _dataContext.PrankCallFromPhoneNumber(request.ToPhoneNumberCountryCodePrefix).ToListAsync();
            var objPrankCallFromPhone = new PrankCallFromPhoneNumberModel();
            if (fromNumber != null && fromNumber.Any())
            {
                objPrankCallFromPhone = fromNumber.FirstOrDefault();
                if (!string.IsNullOrEmpty(objPrankCallFromPhone.CountryCode))
                {
                    TwilioClient.Init(_twilioSettings.TwilioAccountSid, _twilioSettings.TwilioAuthToken);

                    StringBuilder str_build = new StringBuilder();
                    Random random = new Random();

                    int length = 7;
                    char letter;

                    for (int i = 0; i < length; i++)
                    {
                        double flt = random.NextDouble();
                        int shift = Convert.ToInt32(Math.Floor(25 * flt));
                        letter = Convert.ToChar(shift + 65);
                        str_build.Append(letter);
                    }

                    string sessionId = str_build.ToString();
                    request.PrankCallFromId = objPrankCallFromPhone.PrankCallFromId;

                    // http://960d5976e357.ngrok.io/JoinConfrence/


                    string joinConfrenceUrl = _twilioSettings.TwilioApiUrl + "JoinConfrence?call_session_id=" + sessionId;
                    string completeCallUrl = _twilioSettings.TwilioApiUrl + "CompleteCall";

                    var call = CallResource.Create(
                          to: new Twilio.Types.PhoneNumber(objPrankCallFromPhone.CountryCode + request.ToPhoneNumber),
                          from: new Twilio.Types.PhoneNumber(objPrankCallFromPhone.CountryCode + objPrankCallFromPhone.PhoneNumber),//"+918764245892"
                          url: new Uri(joinConfrenceUrl),
                          statusCallbackEvent: new[] { Number.EventEnum.Completed.ToString() }.ToList(),
                          statusCallback: new Uri(completeCallUrl),
                          statusCallbackMethod: Twilio.Http.HttpMethod.Post
                    );

                    sessionID_to_callsid = call.Sid;

                    //  dials the caller (from number) into the conference

                    string confrenceCalleeUrl = _twilioSettings.TwilioApiUrl + "ConfrenceCallee?confrenceFriendlyName=" + sessionId;
                    var CallRequest = JsonConvert.SerializeObject(request);
                    //string confrenceRecordingCallbackUrl = _twilioSettings.TwilioApiUrl + "ConfrenceRecordingCallback?callRequest=" + CallRequest;
                    string confrenceRecordingCallbackUrl = _twilioSettings.TwilioApiUrl + "ConfrenceRecordingCallback?callRequest=d";
                    var dial = new Dial();
                    dial.Conference(name: sessionId,
                         waitUrl: new Uri("https://twimlets.com/holdmusic?Bucket=com.twilio.music.electronica"),
                        statusCallbackEvent: new[] { Conference.EventEnum.Join }.ToList(),
                        beep: Conference.BeepEnum.Onenter,
                                        statusCallback: new Uri(confrenceCalleeUrl),
                                        startConferenceOnEnter: true,
                                         endConferenceOnExit: true,
                                        record: Conference.RecordEnum.RecordFromStart,
                                        trim: Conference.TrimEnum.TrimSilence,
                                        muted: true,
                                        recordingStatusCallback: new Uri(confrenceRecordingCallbackUrl));

                    response.Append(dial);
                }
            }
            return Content(response.ToString());
        }


        // this is an endpoint to add the callee into the conference call
        [HttpPost]
        [Route("JoinConfrence")]
        public ActionResult JoinConfrence(string call_session_id)
        {
            var response = new VoiceResponse();
            string leaveUrl = _twilioSettings.TwilioApiUrl + "Leave";
            var dial = new Dial();
            dial.Conference(name: call_session_id,
                                  statusCallbackEvent: new[] { Conference.EventEnum.Leave }.ToList(),
                                  statusCallback: new Uri(leaveUrl),
                                  endConferenceOnExit: true
                                );
            response.Append(dial);

            return Content(response.ToString());
        }

        // this is an endpoint when callee join the conference call in 'JoinConfrence'. I have specified statusCallback in 'JoinConfrence' but this not calling
        // Here I have copied this 'ConfrenceCallee' endpoint code inside 'JoinConfrence' endpoint for play in case 'ConferenceResource.Update' get stuck
        [HttpGet]
        [Route("ConfrenceCallee")]
        public ActionResult ConfrenceCallee(string confrenceFriendlyName)
        {

            var model = new ResponseModel();
            var noOfParticipants = HttpContext.Request.Form["SequenceNumber"];
            int participants = 0;

            int.TryParse(noOfParticipants, out participants);
            if (HttpContext.Request.Form["StatusCallbackEvent"] == "participant-join" && participants > 1)
            {
                TwilioClient.Init(_twilioSettings.TwilioAccountSid, _twilioSettings.TwilioAuthToken);

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
                string joinConfrenceCallbackUrl = _twilioSettings.TwilioApiUrl + "JoinConfrenceCallback";
                var participant = ConferenceResource.Update(
                      announceUrl: new Uri(joinConfrenceCallbackUrl),
                      pathSid: pathConferenceSid


                );
                model.StatusCode = 200;




            }
            return Json(model);
        }
        [HttpPost]
        [Route("JoinConfrenceCallback")]
        public ActionResult JoinConfrenceCallback()
        {
            var response = new VoiceResponse();
            var play = new Play(url: new Uri("https://prankapibucket.s3.ap-south-1.amazonaws.com/Audio/d88e5d94-d8f8-478d-a92f-8cf4e21d5572.mp3"));
            response.Append(play);
            return Content(response.ToString());
        }

        [HttpGet]
        [Route("ConfrenceRecordingCallback")]
        public async Task<ActionResult> ConfrenceRecordingCallback(string callRequest)
        {
            var model = new ResponseModel();
            if (string.IsNullOrEmpty(HttpContext.Request.Form["RecordingUrl"]))
            {
                string recordingUrl = HttpContext.Request.Form["RecordingUrl"].ToString();
                PrankCallRequestModel request = JsonConvert.DeserializeObject<PrankCallRequestModel>(callRequest);

                var callTrackingInfo = new PrankCallTrackingModel
                {
                    DeviceId = request.DeviceId,
                    PrankId = request.PrankId,
                    PrankCallPoints = request.CostPoint,
                    PrankCallFromId = request.PrankCallFromId,
                    ToPhoneNumberPersonName = request.ToPhoneNumberPersonName,
                    ToPhoneNumber = request.ToPhoneNumber,
                    IsSavePhoneCall = request.IsRecordCall,
                    PlivoRecordCallUrl = recordingUrl + ".mp3",
                    RecordedAudioFile = recordingUrl + ".mp3",
                    AddedDate = DateTime.Now.ToString("yyyy-MM-dd HH:MM:ss"),
                    IsDeleted = false
                };

                var result = await _dataContext.AddPrankCallTracking(callTrackingInfo);
                if (!result.Status)
                {
                    model.StatusCode = 404;
                    model.Message = result.Message;
                }
                else
                {
                    model.StatusCode = 200;
                    model.Message = recordingUrl;
                }
            }
            else
            {
                model.StatusCode = 404;
                model.Message = "Call not Answered";
            }
            return Json(model);
        }
        [HttpPost]
        [Route("CompleteCall")]
        public ActionResult CompleteCall()
        {
            return Content("");
        }

        [HttpGet]
        [Route("Leave")]
        public ActionResult Leave()
        {
            if (HttpContext.Request.Form["StatusCallbackEvent"] == "participant-leave")
            {
                TwilioClient.Init(_twilioSettings.TwilioAccountSid, _twilioSettings.TwilioAuthToken);
            }
            var model = new ResponseModel
            {
                StatusCode = 200
            };
            return Json(model);
        }
        #endregion
    }
}
