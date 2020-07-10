using System;
using System.Linq;
using Microsoft.Extensions.Options;
using PrankAWSWebApp.Utility;
using PrankAWSWebApp.Factory;
using Microsoft.AspNetCore.Identity;
using PrankAWSWebApp.Areas.Admin.Data;
using PrankAWSWebApp.Common;
using Microsoft.AspNetCore.Mvc;
using PrankAWSWebApp.Models;
using System.Threading.Tasks;
using System.Security.Claims;
using PrankAWSWebApp.Areas.Admin.Models;
using System.Collections.Generic;
using Prank.Model;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PrankAWSWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class ReferralInviteInfoController : Controller
    {
        IConfiguration _iconfiguration;
        private readonly IOptions<MySettingsModel> appSettings;
        private readonly SignInManager<PrankIdentityUser> loginManager;
        private readonly UserManager<PrankIdentityUser> userManager;

        public ReferralInviteInfoController(IOptions<MySettingsModel> app, SignInManager<PrankIdentityUser> loginManager, IConfiguration iconfiguration, UserManager<PrankIdentityUser> userManager)
        {
            appSettings = app;
            ApplicationSettings.WebApiUrl = appSettings.Value.WebApiBaseUrl;
            this.loginManager = loginManager;
            this.userManager = userManager;
            _iconfiguration = iconfiguration;
        }
        public async Task<IActionResult> Index()
        {
            var model = new ReferralInviteViewModel()
            {
                DeviceKeyLst = new List<SelectListItem>()
            };

            string token = string.Empty;
            var claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
            var userTokenClaim = claimsIdentity.Claims.SingleOrDefault(c => c.Type == "Token");
            if (userTokenClaim != null)
            {
                token = userTokenClaim.Value;
            }
            try
            {
                var filter = new FilterModel();

                filter.Token = token;
                  

                var data = await ApiClientFactory.Instance.GetDeviceInfo(filter);
                if (data != null && data.Any())
                {
                    foreach (var item in data)
                    {
                        model.DeviceKeyLst.Add(new SelectListItem
                        {
                            Text = item.DeviceKey,
                            Value = item.DeviceId.ToString()
                        });
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return View(model);

        }

        //public async Task<IActionResult> PartialGetReferralInviteInfoLst(string search, int searchByDeviceId)
        //{

        //    string token = string.Empty;
        //    var claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
        //    var userTokenClaim = claimsIdentity.Claims.SingleOrDefault(c => c.Type == "Token");
        //    if (userTokenClaim != null)
        //    {
        //        token = userTokenClaim.Value;
        //    }
        //    try
        //    {
        //        var data = await ApiClientFactory.Instance.GetReferralInviteInfo(token, searchByDeviceId, search, 0, 1000);
        //        string viewFromCurrentController = await this.RenderViewToStringAsync("PartialGetReferralInviteInfoLst", data);

        //        return Json(
        //           new
        //           {
        //               html = viewFromCurrentController
        //           });
        //    }
        //    catch (Exception e)
        //    {

        //        throw;
        //    }
        //}


        public async Task<IActionResult> PartialGetReferralInviteInfoLst(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch)
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
                            sortCol = "FromDeviceId";
                            break;
                        }
                    case 1:
                        {
                            sortCol = "ToDeviceId";
                            break;
                        }
                    case 2:
                        {
                            sortCol = "FreeToken";
                            break;
                        }
                    case 3:
                        {
                            sortCol = "Status";
                            break;
                        }
                    case 4:
                        {
                            sortCol = "ReferDate";
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
                var data = await ApiClientFactory.Instance.GetReferralInviteInfo(filter);

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


    }
}
