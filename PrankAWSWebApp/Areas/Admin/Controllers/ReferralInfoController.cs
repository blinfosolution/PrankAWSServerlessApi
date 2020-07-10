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

namespace PrankAWSWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class ReferralInfoController : Controller
    {
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

        public ReferralInfoController(IOptions<MySettingsModel> app, SignInManager<PrankIdentityUser> loginManager, UserManager<PrankIdentityUser> userManager)
        {
            appSettings = app;
            ApplicationSettings.WebApiUrl = appSettings.Value.WebApiBaseUrl;
            this.loginManager = loginManager;
            this.userManager = userManager;

        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> PartialReferralInfoLst()
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
                var data = await ApiClientFactory.Instance.GetReferralInfo(token, 0, 0, 1000);
                string viewFromCurrentController = await this.RenderViewToStringAsync("PartialReferralInfoLst", data);

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
        public async Task<IActionResult> PartialAddEditReferralInfo(int referralId)
        {
            var model = new ReferralInfoAddEditModel();
            if (referralId > 0)
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
                    var result = await ApiClientFactory.Instance.GetReferralInfo(token, referralId, 0, 1000); ;
                    if (result != null & result.Count > 0)
                    {
                        foreach (var item in result)
                        {
                            model.ReferralId = item.ReferralId;
                            model.ReferralTitle = item.ReferralTitle;
                            model.ReferralDesc = item.Description;
                            model.ReferralCode = item.ReferralCode;
                            model.FreeCreditPoint = item.FreeCreditPoint;
                            model.IsActive = item.IsActive;
                            model.AddedDate = item.AddedDate;
                        }
                    }
                }
                catch (Exception e)
                {
                }
            }
            string viewFromCurrentController = await this.RenderViewToStringAsync("PartialAddEditReferralInfo", model);
            return Json(new { html = viewFromCurrentController });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SaveUpdate(ReferralInfoAddEditModel model)
        {
            string token = string.Empty;
            var claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
            var userTokenClaim = claimsIdentity.Claims.SingleOrDefault(c => c.Type == "Token");
            if (userTokenClaim != null)
            {
                token = userTokenClaim.Value;
            }
            var data = new ResponseModel();
            var objResponse = new SaveResponse();
            string viewFromCurrentController = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    var refModel = new ReferralInfoModel()
                    {
                        ReferralTitle=model.ReferralTitle,
                        Description=model.ReferralDesc,
                        ReferralId = model.ReferralId,
                        ReferralCode = model.ReferralCode,
                        FreeCreditPoint = model.FreeCreditPoint,
                        IsActive = model.IsActive,
                     };
                    if (model.ReferralId > 0)
                    {
                        data = await ApiClientFactory.Instance.UpdateReferralInfo(refModel);
                    }
                    else
                    {
                        data = await ApiClientFactory.Instance.SaveReferralInfo(refModel);
                    }
                    if (data.ResponseObject != null)
                    {
                        var referralLst = JsonConvert.DeserializeObject<List<ReferralInfoListModel>>(data.ResponseObject.ToString());
                        viewFromCurrentController = await this.RenderViewToStringAsync("PartialReferralInfoLst", referralLst);
                    }
                    objResponse.StatusCode = Convert.ToInt32(data.StatusCode);
                    objResponse.Html = viewFromCurrentController;
                    if (Convert.ToInt32(data.StatusCode) == 200)
                    {
                        objResponse.Message = model.ReferralId > 0 ? "Record updated successfully" : "Record saved successfully";
                        await TrackingInfo.TrackInfo(token, EmployeeId, ControllerContext.ActionDescriptor.ControllerName, model.ReferralTitle,
                          model.ReferralId, model.ReferralId > 0 ? "Update" : "Save");
                    }
                    else
                    {
                        objResponse.Message = model.ReferralId > 0 ? "Record not updated successfully" : "Record not saved successfully";
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

        [HttpGet]
        public async Task<JsonResult> DeleteReferralInfo(int id)
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
                bool isDeleted = await ApiClientFactory.Instance.DeleteReferralInfo(token, id);
                if (isDeleted)
                {
                    objResponse.StatusCode = 200;
                    objResponse.Message = "Record deleted successfully";
                    await TrackingInfo.TrackInfo(token, EmployeeId, ControllerContext.ActionDescriptor.ControllerName, id.ToString(), id, "Delete");
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
