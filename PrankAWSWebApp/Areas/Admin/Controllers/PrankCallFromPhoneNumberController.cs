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
    public class PrankCallFromPhoneNumberController : Controller
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

        public PrankCallFromPhoneNumberController( IOptions<MySettingsModel> app, SignInManager<PrankIdentityUser> loginManager, UserManager<PrankIdentityUser> userManager)
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
        public async Task<IActionResult> PartialCallFromPhoneNoLst(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch)
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
                            sortCol = "Country";
                            break;
                        }
                    case 1:
                        {
                            sortCol = "CountryCode";
                            break;
                        }
                    case 2:
                        {
                            sortCol = "PhoneNumber";
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
                    case 5:
                        {
                            sortCol = "ModifiedDate";
                            break;
                        }

                    default:
                        {
                            sortCol = "PrankCallFromId";
                            break;
                        }
                }
                var data = await ApiClientFactory.Instance.GetPrankCallFromPhoneNoLst(token, 0, iDisplayStart, iDisplayLength, sSearch, sortCol, sSortDir_0);
                
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

        public async Task<IActionResult> PartialAddEditCallFromNumber(int fromId)
        {
            var model = new PrankCallFromPhoneNumberAddEditModel();
          

            if (fromId > 0)
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
                    var result = await ApiClientFactory.Instance.GetPrankCallFromPhoneNoLst(token, fromId, 0, 1000000, string.Empty, string.Empty,string.Empty); ;
                    if (result != null & result.Count > 0)
                    {
                        foreach (var item in result)
                        {
                            model.PrankCallFromId = item.PrankCallFromId;
                            model.Country = item.Country;
                            model.CountryCode = item.CountryCode;
                            model.PhoneNumber = item.PhoneNumber;
                            model.IsActive = item.IsActive;
                        }
                    }
                }
                catch (Exception e)
                {
                }
            }
            string viewFromCurrentController = await this.RenderViewToStringAsync("PartialAddEditCallFromNumber", model);
            return Json(new { html = viewFromCurrentController });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SaveUpdate(PrankCallFromPhoneNumberAddEditModel model)
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
                    var FromPhNoModel = new PrankCallFromPhoneNumberModel()
                    {
                        PrankCallFromId = model.PrankCallFromId,
                        Country = model.Country,
                        CountryCode = model.CountryCode,
                        PhoneNumber = model.PhoneNumber,
                        IsActive = model.IsActive
                    };
                    if (model.PrankCallFromId > 0)
                    {
                        data = await ApiClientFactory.Instance.UpdatePrankCallFromPhoneNo(FromPhNoModel);
                    }
                    else
                    {
                        data = await ApiClientFactory.Instance.SavePrankCallFromPhoneNo(FromPhNoModel);
                    }                   
                    objResponse.StatusCode = Convert.ToInt32(data.StatusCode);
                   
                    if (Convert.ToInt32(data.StatusCode) == 200)
                    {
                        objResponse.Message = model.PrankCallFromId > 0 ? "Record updated successfully" : "Record saved successfully";
                        await TrackingInfo.TrackInfo(token, EmployeeId, ControllerContext.ActionDescriptor.ControllerName, model.PhoneNumber,
                          model.PrankCallFromId, model.PrankCallFromId > 0 ? "Update" : "Save");
                    }
                    else
                    {
                        objResponse.Message = model.PrankCallFromId > 0 ? "Record not updated successfully" : "Record not saved successfully";
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
        public async Task<JsonResult> DeletePrankCallFromPhoneNumber(int id)
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
                bool isDeleted = await ApiClientFactory.Instance.DeletePrankCallFromPhoneNo(token, id);
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


        public async Task<JsonResult> ChangeStatusFromPhoneNumber(int PrankCallFromId, bool IsDefault)
        {
            IsDefault = IsDefault ? false : true;
            var data = new ResponseModel();
            var objResponse = new SaveResponse();
            string viewFromCurrentController = string.Empty;
            if (PrankCallFromId > 0)
            {
                string token = string.Empty;
                var claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
                var userTokenClaim = claimsIdentity.Claims.SingleOrDefault(c => c.Type == "Token");
                if (userTokenClaim != null)
                {
                    token = userTokenClaim.Value;
                }
                data = await ApiClientFactory.Instance.ChangeStatusPrankCallFromPhoneNumber(token, PrankCallFromId, IsDefault);

                objResponse.StatusCode = Convert.ToInt32(data.StatusCode);

                if (objResponse.StatusCode == 200)
                {
                    objResponse.Message = "Record status changed successfully";
                    await TrackingInfo.TrackInfo(token, EmployeeId, ControllerContext.ActionDescriptor.ControllerName, PrankCallFromId.ToString(), PrankCallFromId, "ChangeStatusFromPhoneNumber");
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
    }
}
