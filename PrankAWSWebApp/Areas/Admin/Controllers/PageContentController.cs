using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prank.Model;
using PrankAWSWebApp.Areas.Admin.Models;
using PrankAWSWebApp.Controllers;
using PrankAWSWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PrankAWSWebApp.Utility;
using PrankAWSWebApp.Factory;
using Microsoft.AspNetCore.Identity;
using PrankAWSWebApp.Areas.Admin.Data;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using PrankAWSWebApp.Common;

namespace PrankAWSWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class PageContentController : Controller
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

        public PageContentController(IOptions<MySettingsModel> app, SignInManager<PrankIdentityUser> loginManager, IConfiguration iconfiguration, UserManager<PrankIdentityUser> userManager)
        {
            appSettings = app;
            ApplicationSettings.WebApiUrl = appSettings.Value.WebApiBaseUrl;
            this.loginManager = loginManager;
            this.userManager = userManager;
            _iconfiguration = iconfiguration;
        }
        public async Task<IActionResult> Index()
        {
            string token = string.Empty;
            var claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
            var userTokenClaim = claimsIdentity.Claims.SingleOrDefault(c => c.Type == "Token");
            if (userTokenClaim != null)
            {
                token = userTokenClaim.Value;
            }
            var data = await ApiClientFactory.Instance.GetPageContents(token);
            return View(data);
        }

        public async Task<IActionResult> PartialGetPageContent()
        {
            string token = string.Empty;
            var claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
            var userTokenClaim = claimsIdentity.Claims.SingleOrDefault(c => c.Type == "Token");
            if (userTokenClaim != null)
            {
                token = userTokenClaim.Value;
            }
            var data = await ApiClientFactory.Instance.GetPageContents(token);

            string viewFromCurrentController = await this.RenderViewToStringAsync("PartialGetPageContent", data);

            return Json(
               new
               {
                   html = viewFromCurrentController
               });
        }

        public async Task<IActionResult> PartialEdit(int id)
        {
            string viewFromCurrentController = "";
            var model = new PageContentModelViewModel();
            try
            {
                string token = string.Empty;
                var claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
                var userTokenClaim = claimsIdentity.Claims.SingleOrDefault(c => c.Type == "Token");
                if (userTokenClaim != null)
                {
                    token = userTokenClaim.Value;
                }
                model = await ApiClientFactory.Instance.GetPageContentId(token, id);
                viewFromCurrentController = await this.RenderViewToStringAsync("PartialEdit", model);
            }
            catch (Exception ex)
            {
                string ss = ex.ToString();
            }
            return Json(
               new
               {
                   html = viewFromCurrentController
               });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SaveUpdate(PageContentModelViewModel pageContentModelView)
        {
            var data = new PageContentDataModel();
            var objResponse = new SaveResponse();
            string viewFromCurrentController = string.Empty;
            List<string> lstString = new List<string>
                    {
                        string.Empty
                    };
            if (ModelState.IsValid)
            {
                try
                {
                    string token = string.Empty;
                    var claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
                    var userTokenClaim = claimsIdentity.Claims.SingleOrDefault(c => c.Type == "Token");
                    if (userTokenClaim != null)
                    {
                        token = userTokenClaim.Value;
                    }
                    var model = new PageContentModel()
                    {
                       
                        IsActive = pageContentModelView.IsActive,
                        PageContent= pageContentModelView.PageContent,
                        PageContentKey= pageContentModelView.PageContentKey,
                        PageContentId= pageContentModelView.PageContentId
                    };
                    
                        data = await ApiClientFactory.Instance.UpdatePageContent(token, model);
                     

                    if (data.responseObject != null && data.responseObject.Any())
                    {
                        viewFromCurrentController = await this.RenderViewToStringAsync("PartialGetPageContent", data.responseObject);
                    }
                    objResponse.StatusCode = Convert.ToInt32(data.statusCode);
                    objResponse.Html = viewFromCurrentController;
                    if (Convert.ToInt32(data.statusCode) == 200)
                    {
                        objResponse.Message = "Record saved successfully";
                        await TrackingInfo.TrackInfo(token, EmployeeId, ControllerContext.ActionDescriptor.ControllerName, pageContentModelView.PageContentKey,
                          pageContentModelView.PageContentId, pageContentModelView.PageContentId > 0 ? "Update" : "Save");
                    }
                    else
                    {
                        objResponse.Message = "Record not saved successfully";
                    }

                    return new JsonResult(new
                    {
                        objResponse
                    });
                }
                catch (Exception ex)
                {
                    string exep = ex.ToString();
                }
            }
            return new JsonResult(new List<string>());
        }
    }
}
