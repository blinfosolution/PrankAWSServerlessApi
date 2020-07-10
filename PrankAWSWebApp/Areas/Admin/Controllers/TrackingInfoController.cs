using System;
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
using PrankAWSWebApp.Areas.Admin.Models;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Prank.Model;
using Amazon.S3.Transfer;
using Amazon.S3;
using Amazon;
using Microsoft.AspNetCore.Http;

namespace PrankAWSWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class TrackingInfoController : Controller
    {
        IConfiguration _iconfiguration;
        private readonly IOptions<MySettingsModel> appSettings;
        private readonly SignInManager<PrankIdentityUser> loginManager;
        private readonly UserManager<PrankIdentityUser> userManager;
     
        public TrackingInfoController(IOptions<MySettingsModel> app, SignInManager<PrankIdentityUser> loginManager, IConfiguration iconfiguration, UserManager<PrankIdentityUser> userManager)
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

        public async Task<IActionResult> PartialTrackingInfoLst(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch)
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
                            sortCol = "FirstName";
                            break;
                        }
                    case 1:
                        {
                            sortCol = "UserName";
                            break;
                        }
                    case 2:
                        {
                            sortCol = "ModuleName";
                            break;
                        }
                    case 3:
                        {
                            sortCol = "WorkDescription";
                            break;
                        }
                    case 4:
                        {
                            sortCol = "trackDate";
                            break;
                        }

                    default:
                        {
                            sortCol = "TrackingId";
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
                var data = await ApiClientFactory.Instance.GetTrackingInfo(filter);

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
