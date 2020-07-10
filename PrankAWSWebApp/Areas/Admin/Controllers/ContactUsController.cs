using PrankAWSWebApp.Factory;
using Microsoft.AspNetCore.Identity;
using PrankAWSWebApp.Areas.Admin.Data;
using Microsoft.Extensions.Configuration;
using PrankAWSWebApp.Common;
using Microsoft.AspNetCore.Mvc;
using PrankAWSWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using PrankAWSWebApp.Utility;
using System.Threading.Tasks;
using System.Security.Claims;
using Prank.Model;

namespace PrankAWSWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class ContactUsController : Controller
    {
        IConfiguration _iconfiguration;
        private readonly IOptions<MySettingsModel> appSettings;
        private readonly SignInManager<PrankIdentityUser> loginManager;
        private readonly UserManager<PrankIdentityUser> userManager;

        public ContactUsController(IOptions<MySettingsModel> app, SignInManager<PrankIdentityUser> loginManager, IConfiguration iconfiguration, UserManager<PrankIdentityUser> userManager)
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
        public async Task<IActionResult> PartialContactUsLst(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch)
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
                            sortCol = "EmailTo";
                            break;
                        }
                    case 1:
                        {
                            sortCol = "Message";
                            break;
                        }
                    case 2:
                        {
                            sortCol = "SendDate";
                            break;
                        }
                    default:
                        {
                            sortCol = "EmailTrackId";
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
                var data = await ApiClientFactory.Instance.GetContackUsLst(filter);


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
