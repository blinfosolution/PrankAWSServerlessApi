using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Prank.DAL;
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

namespace PrankAWSServerlessApi.Controllers
{
    //  [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PrankCallTrackingController : BaseApiController
    {
        private readonly TwilioSettings _twilioSettings;
        private IWebHostEnvironment hostEnvironment;
        private string sessionID_to_callsid = string.Empty;
        public PrankCallTrackingController(DataContext dataContext, IConfiguration config, FileSettings fileSettings, TwilioSettings TwilioSettings, IWebHostEnvironment env) : base(dataContext, config, fileSettings, env)
        {
            _twilioSettings = TwilioSettings;
            hostEnvironment = env;
        }

        //[HttpPost]
        //[Route("PrankCallRequest")]
        //public async Task<ActionResult<ResponseModel>> CallByRequestParam(PrankCallRequestModel request)
        //{
        //    var model = new ResponseModel();
        //    var fromNumber = await _dataContext.PrankCallFromPhoneNumber(request.ToPhoneNumberCountryCodePrefix).ToListAsync();
        //    var objPrankCallFromPhone = new PrankCallFromPhoneNumberModel();
        //    if (fromNumber != null && fromNumber.Any())
        //    {
        //        objPrankCallFromPhone = fromNumber.FirstOrDefault();
        //        if (!string.IsNullOrEmpty(objPrankCallFromPhone.CountryCode))
        //        {
        //            try
        //            {
        //                #region Getxml from prankinfo
        //                string voiceXml = string.Empty;
        //                var prankinfo = _dataContext.GetPrankInfoById(request.PrankId);

        //                if (prankinfo != null)
        //                {
        //                    foreach (var info in prankinfo)
        //                    {
        //                        voiceXml = info.PlivoAudioXmlUrl;
        //                    }
        //                }
        //                #endregion


        //                if (!string.IsNullOrEmpty(voiceXml))
        //                { 
        //                    #region Call Plivo
        //                    var api = new PlivoApi(_plivoSettings.PlivoAuthId, _plivoSettings.PlivoAuthToken);
        //                    var response = api.Call.Create(
        //                        to: new List<string> { objPrankCallFromPhone.CountryCode + request.ToPhoneNumber },
        //                        from: objPrankCallFromPhone.CountryCode + objPrankCallFromPhone.PhoneNumber,
        //                        answerMethod: "GET",
        //                        answerUrl: voiceXml,
        //                        timeLimit: 144000
        //                    );

        //                    string RecordingUrl = string.Empty;
        //                    Thread.Sleep(2000);
        //                    try
        //                    {
        //                        var startRecord = api.Call.StartRecording(
        //                           callUuid: response.RequestUuid
        //                       );
        //                        RecordingUrl = startRecord.Url;
        //                    }
        //                    catch (PlivoRestException e)
        //                    {
        //                        model.StatusCode = 404;
        //                        model.Message = e.Message;
        //                    }

        //                    #endregion

        //                #region Call Status
        //                    int i = 1;
        //                    bool IsAnswered = false;
        //                    while (i > 0)
        //                    {
        //                        var responseLive = api.Call.GetLive(liveCallUuid: response.RequestUuid);
        //                        i++;
        //                        if (responseLive.CallStatus == "in-progress" || responseLive.CallStatus.ToString() == "completed")
        //                        {
        //                            i = 0;
        //                            IsAnswered = true;
        //                        }
        //                    }
        //                    #endregion

        //                #region Save Data in PrankCallTracking
        //                    if (IsAnswered)
        //                    {
        //                        var callTrackingInfo = new PrankCallTrackingModel
        //                        {
        //                            DeviceId = request.DeviceId,
        //                            PrankId = request.PrankId,
        //                            PrankCallPoints = request.CostPoint,
        //                            PrankCallFromId = objPrankCallFromPhone.PrankCallFromId,
        //                            ToPhoneNumberPersonName = request.ToPhoneNumberPersonName,
        //                            ToPhoneNumber = request.ToPhoneNumber,
        //                            IsSavePhoneCall = request.IsRecordCall,
        //                            PlivoRecordCallUrl = RecordingUrl,
        //                            RecordedAudioFile = RecordingUrl,
        //                            AddedDate = DateTime.Now.ToString("yyyy-MM-dd HH:MM:ss"),
        //                            IsDeleted = false
        //                        };

        //                        var result = await _dataContext.AddPrankCallTracking(callTrackingInfo);
        //                        if (!result.Status)
        //                        {
        //                            model.StatusCode = 404;
        //                            model.Message = result.Message;
        //                        }
        //                        else
        //                        {
        //                            model.StatusCode = 200;
        //                            model.Message = RecordingUrl;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        model.StatusCode = 404;
        //                        model.Message = "Call not Answered";

        //                    }


        //                    #endregion
        //                }
        //            }
        //            catch (PlivoRestException e)
        //            {
        //                model.StatusCode = 404;
        //                model.Message = e.Message;
        //            }
        //        }
        //    }

        //    return model;
        //}

        #region List
        [HttpGet]
        [Route("GetPrankCallHistory")]
        public async Task<ActionResult<ResponseModel>> GetCallHistoryListByRequest(int deviceId, int trackingId, int skip, int take)
        {
            var model = new ResponseModel();

            var objPrankList = await _dataContext.GetPrankCallHistoryList(deviceId, trackingId, skip, take).ToListAsync();

            if (objPrankList != null)
            {
                model.ResponseObject = objPrankList;
                model.StatusCode = 200;
            }
            else
            {
                model.StatusCode = 200;
            }

            return model;
        }

        #endregion
        #region Delete
        [HttpDelete]
        [Route("DeletePrankCallHistory")]
        public async Task<ActionResult<ResponseModel>> DeleteGet(int trackingId)
        {
            var model = new ResponseModel();
            var result = await _dataContext.DeleteCallHistory(trackingId);

            if (!result.Status)
            {
                model.StatusCode = 404;
                model.Message = result.Message;
            }
            else
            {
                model.StatusCode = 200;
            }

            return model;
        }

        #endregion


     
         }
}