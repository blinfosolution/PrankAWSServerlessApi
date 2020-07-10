using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using PrankAWSWebApp.Utility;
using PrankAWSWebApp.Factory;
using Microsoft.AspNetCore.Identity;
using PrankAWSWebApp.Areas.Admin.Data;
using Microsoft.Extensions.Configuration;
using PrankAWSWebApp.Common;
using Microsoft.AspNetCore.Mvc;
using PrankAWSWebApp.Models;
using System.Threading.Tasks;
using System.Security.Claims;
using Prank.Model;
using Newtonsoft.Json;
using System.IO;
using System.Net;

namespace PrankAWSWebApp.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class DeviceInfoController : Controller
    {
        IConfiguration _iconfiguration;
        private readonly IOptions<MySettingsModel> appSettings;
        private readonly SignInManager<PrankIdentityUser> loginManager;
        private readonly UserManager<PrankIdentityUser> userManager;
        private Task<PrankIdentityUser> GetCurrentUserAsync() => userManager.GetUserAsync(HttpContext.User);

        public string EmployeeId
        {
            get
            {
                //var usr = await GetCurrentUserAsync();
                var task = Task.Run(async () => await GetCurrentUserAsync());
                return task.Result.Id;
                //return usr?.Id;
            }
        }

        public DeviceInfoController(IOptions<MySettingsModel> app, SignInManager<PrankIdentityUser> loginManager, IConfiguration iconfiguration, UserManager<PrankIdentityUser> userManager)
        {
            appSettings = app;
            ApplicationSettings.WebApiUrl = appSettings.Value.WebApiBaseUrl;
            this.loginManager = loginManager;
            this.userManager = userManager;
            _iconfiguration = iconfiguration;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> PartialGetDeviceinfoLst(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch)
        {
            string token = string.Empty;
            var claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
            var userTokenClaim = claimsIdentity.Claims.SingleOrDefault(c => c.Type == "Token");
            if (userTokenClaim != null)
            {
                token = userTokenClaim.Value;
            }
            try
            {
                string sortCol = string.Empty;
                switch (iSortCol_0)
                {

                    case 0:
                        {
                            sortCol = "DeviceKey";
                            break;
                        }
                    case 1:
                        {
                            sortCol = "DeviceInfomation";
                            break;
                        }
                    case 3:
                        {
                            sortCol = "IsActive";
                            break;
                        }
                    case 4:
                        {
                            sortCol = "AddedDate";
                            break;
                        }

                    default:
                        {
                            sortCol = "DeviceId";
                            break;
                        }
                }
                var filter = new FilterModel()
                {
                    Token = token,
                    Skip = iDisplayStart,
                    Take = iDisplayLength,
                    Search = sSearch,
                    SortCol = sortCol,
                    SortDir = sSortDir_0
                };
                var data = await ApiClientFactory.Instance.GetDeviceInfo(filter);
                
                var result = new
                {
                    iTotalRecords = data.Any() ? data.FirstOrDefault().TotalCount : 0,
                    iTotalDisplayRecords = data.Any() ? data.FirstOrDefault().TotalCount : 0,
                    aaData = data
                };
                return Json(result);
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public async Task<JsonResult> ChangeStatusDeviceInfo(int deviceId, bool isActive)
        {
            isActive = isActive ? false : true;
            var data = new ResponseModel();
            var objResponse = new SaveResponse();
            string viewFromCurrentController = string.Empty;
            if (deviceId > 0)
            {
                string token = string.Empty;
                var claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
                var userTokenClaim = claimsIdentity.Claims.SingleOrDefault(c => c.Type == "Token");
                if (userTokenClaim != null)
                {
                    token = userTokenClaim.Value;
                }
                data = await ApiClientFactory.Instance.ChangeStatusDeviceInfo(token, deviceId, isActive);

                objResponse.StatusCode = Convert.ToInt32(data.StatusCode);
                
                if (objResponse.StatusCode == 200)
                {
                    await TrackingInfo.TrackInfo(token, EmployeeId, ControllerContext.ActionDescriptor.ControllerName, deviceId.ToString(),
                          deviceId, "ChangeStatusDevice");
                    objResponse.Message = "Record status changed successfully";
                }
                else
                {
                    objResponse.Message = "Record not status changed successfully";
                }
            }
            return new JsonResult(new
            {
                objResponse
            });


        }
        public async Task<JsonResult> GetCallHistoryByDeviceId(int deviceId)
        {

            string token = string.Empty;
            var claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
            var userTokenClaim = claimsIdentity.Claims.SingleOrDefault(c => c.Type == "Token");
            if (userTokenClaim != null)
            {
                token = userTokenClaim.Value;
            }
            var data = await ApiClientFactory.Instance.GetPrankCallHistoryLst(token, deviceId, 0, 10000);
            string viewFromCurrentController = await this.RenderViewToStringAsync("GetCallHistoryByDeviceId", data);
            return Json(
                new
                {
                    html = viewFromCurrentController
                });
        }
        public async Task<JsonResult> GetPackageOrderByDeviceId(int deviceId)
        {

            string token = string.Empty;
            var claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
            var userTokenClaim = claimsIdentity.Claims.SingleOrDefault(c => c.Type == "Token");
            if (userTokenClaim != null)
            {
                token = userTokenClaim.Value;
            }
            var data = await ApiClientFactory.Instance.GetPackageOrderLst(token, deviceId);
            string viewFromCurrentController = await this.RenderViewToStringAsync("GetPackageOrderByDeviceId", data);
            return Json(
                new
                {
                    html = viewFromCurrentController
                });
        }

        public FileStreamResult DownLoadAudioFile(string url)
        {
            Stream rtn = null;
            HttpWebRequest aRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse aResponse = (HttpWebResponse)aRequest.GetResponse();
            rtn = aResponse.GetResponseStream();
            return File(rtn, "audio/mpeg", "RecodedAudio.mp3");
        }

    }
}
