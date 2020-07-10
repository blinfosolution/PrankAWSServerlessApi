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
using System;
using Microsoft.AspNetCore.Authorization;

namespace PrankAWSWebApp.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class CallTrackingController : Controller
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
                var task = Task.Run(async () => await GetCurrentUserAsync());
                return task.Result.Id;
            }
        }

        public CallTrackingController(IOptions<MySettingsModel> app, SignInManager<PrankIdentityUser> loginManager, IConfiguration iconfiguration, UserManager<PrankIdentityUser> userManager)
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

        public async Task<IActionResult> PartialCallTrackingLst()
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
                var data = await ApiClientFactory.Instance.GetPrankCallHistoryLst(token,0,0,1000);
                string viewFromCurrentController = await this.RenderViewToStringAsync("PartialCallTrackingLst", data);

                return Json(
                   new
                   {
                       html = viewFromCurrentController
                   });
            }
            catch (Exception e)
            {

                throw;
            }

        }

        [HttpGet]    
        public async Task<JsonResult> DeleteCallHistory(int id)
        {
            
            var objResponse = new SaveResponse();
            string viewFromCurrentController = string.Empty;
            if (id > 0)
            {
                string token = string.Empty;
                var claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
                var userTokenClaim = claimsIdentity.Claims.SingleOrDefault(c => c.Type == "Token");
                if (userTokenClaim != null)
                {
                    token = userTokenClaim.Value;
                }
                bool isDeleted = await ApiClientFactory.Instance.DeletePrankCallHistory(token, id);

                
                if (isDeleted)
                {
                    await TrackingInfo.TrackInfo(token, EmployeeId, ControllerContext.ActionDescriptor.ControllerName, id.ToString(), id, "Delete");
                    objResponse.StatusCode = 200;
                    objResponse.Message = "Record deleted successfully";
                }
                else
                {
                    objResponse.Message = "Record not deleted successfully";
                }
            }
            else
            {
                objResponse.Message = "Please click on package";
            }

            return new JsonResult(new
            {
                objResponse
            });


        }
    }
}
