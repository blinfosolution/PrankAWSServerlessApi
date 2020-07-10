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
using Newtonsoft.Json;

namespace PrankAWSWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class PackageController : Controller
    {
        IConfiguration _iconfiguration;
        private readonly IOptions<MySettingsModel> appSettings;
        private readonly SignInManager<PrankIdentityUser> loginManager;
        private readonly UserManager<PrankIdentityUser> userManager;
        private Task<PrankIdentityUser> GetCurrentUserAsync() => userManager.GetUserAsync(HttpContext.User);

        public string EmployeeId { 
            get {
                //var usr = await GetCurrentUserAsync();
                var task = Task.Run(async () => await GetCurrentUserAsync());
                return task.Result.Id;
                //return usr?.Id;
            }
        }
        public PackageController(IOptions<MySettingsModel> app, SignInManager<PrankIdentityUser> loginManager, IConfiguration iconfiguration, UserManager<PrankIdentityUser> userManager)
        {
            appSettings = app;
            ApplicationSettings.WebApiUrl = appSettings.Value.WebApiBaseUrl;
            this.loginManager = loginManager;
            this.userManager = userManager;
            _iconfiguration = iconfiguration;


        }
        // GET: PackageController
        public async Task<IActionResult> Index()
        {
            return View();
        }



        // GET: PackageController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PackageController/Create
        public ActionResult Create()
        {
            return View();
        }

        //public async Task<IActionResult> PartialGetPackages()
        //{
        //    string token = string.Empty;
        //    var claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
        //    var userTokenClaim = claimsIdentity.Claims.SingleOrDefault(c => c.Type == "Token");
        //    if (userTokenClaim != null)
        //    {
        //        token = userTokenClaim.Value;
        //    }


        //    var data = await ApiClientFactory.Instance.GetPackages(token);

        //    string viewFromCurrentController = await this.RenderViewToStringAsync("PartialGetPackages", data);

        //    return Json(
        //       new
        //       {
        //           html = viewFromCurrentController
        //       });
        //}

        public async Task<IActionResult> PartialGetPackages(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0)
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
                            sortCol = "PackageTitle";
                            break;
                        }
                    case 1:
                        {
                            sortCol = "TokenCreditsPoint";
                            break;
                        }
                    case 2:
                        {
                            sortCol = "PackagePrice";
                            break;
                        }
                    default:
                        {
                            sortCol = "PackageId";
                            break;
                        }
                }
                var filter = new FilterModel()
                {
                    Token = token,
                    Skip = iDisplayStart,
                    Take = iDisplayLength,
                   // Search = sSearch,
                    SortCol = sortCol,
                    SortDir = sSortDir_0
                };
                var data = await ApiClientFactory.Instance.GetPackages(filter);

                if (data.responseObject != null)
                {
                   // var data = JsonConvert.DeserializeObject<List<PackagesModelViewModel>>(result.responseObject.ToString());

                    var results = new
                    {
                        iTotalRecords = data.responseObject.Any() ? data.responseObject.FirstOrDefault().TotalCount : 0,
                        iTotalDisplayRecords = data.responseObject.Any() ? data.responseObject.FirstOrDefault().TotalCount : 0,
                        aaData = data.responseObject
                    };
                    return Json(results);
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return Json(new { });
        }

        public async Task<IActionResult> PartialCreate()
        {
            var model = new PackagesModelViewModel();
            //string viewFromAnotherController = await this.RenderViewToStringAsync("/Views/News/_NewsList.cshtml", newsItem);
            string viewFromCurrentController = await this.RenderViewToStringAsync("PartialCreate", model);

            return Json(
               new
               {
                   html = viewFromCurrentController
               });
        }
        public async Task<IActionResult> PartialEdit(int id)
        {
            string viewFromCurrentController = "";
            var model = new PackagesModelViewModel();
            try
            {
                string token = string.Empty;
                var claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
                var userTokenClaim = claimsIdentity.Claims.SingleOrDefault(c => c.Type == "Token");
                if (userTokenClaim != null)
                {
                    token = userTokenClaim.Value;
                }
                model = await ApiClientFactory.Instance.GetPackageById(token, id);
                viewFromCurrentController = await this.RenderViewToStringAsync("PartialEdit", model);
            }
            catch(Exception ex)
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
        public async Task<JsonResult> SaveUpdate(PackagesModelViewModel packagesModel)
        {
            var data = new PackagesDataModel();
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
                    var model = new PackagesModel()
                    {
                        AddedDate = DateTime.UtcNow,// Convert.ToDateTime("2020-06-19T18:44:40.633Z"),
                        IsActive = packagesModel.IsActive,
                        ModifiedDate = DateTime.UtcNow,// Convert.ToDateTime("2020-06-19T18:44:40.633Z"),
                        PackageId = packagesModel.PackageId,
                        PackagePrice = packagesModel.PackagePrice,
                        PackageTitle = packagesModel.PackageTitle,
                        ProductIdentifier = packagesModel.ProductIdentifier,
                        SortOrder = packagesModel.SortOrder,
                        TokenCreditsPoint = packagesModel.TokenCreditsPoint
                    };
                    if (model.PackageId > 0)
                    {
                        data = await ApiClientFactory.Instance.UpdatePackages(token, model);
                        
                    }
                    else
                    {
                        data = await ApiClientFactory.Instance.SavePackages(token, model);
                       
                    }

                   

                    if (data.responseObject != null && data.responseObject.Any())
                    {
                        viewFromCurrentController = await this.RenderViewToStringAsync("PartialGetPackages", data.responseObject);
                    }
                    objResponse.StatusCode = Convert.ToInt32(data.statusCode);
                    objResponse.Html = viewFromCurrentController;
                    if (Convert.ToInt32(data.statusCode)==200)
                    {
                        objResponse.Message="Record saved successfully";
                        await TrackingInfo.TrackInfo(token, EmployeeId, ControllerContext.ActionDescriptor.ControllerName, model.PackageTitle,
                            model.PackageId, model.PackageId>0?"Update":"Save");
                    }
                    else
                    {
                        objResponse.Message = "Record not saved successfully";
                    }

                    return new JsonResult(new {
                        objResponse
                    });
                }
                catch(Exception ex)
                {
                    string exep = ex.ToString();
                }
            }
            return new JsonResult(new List<string>());
        }

        [HttpGet]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> Delete(int id)
        {
            var data = new PackagesDataModel();
            var objResponse = new SaveResponse();
            string viewFromCurrentController = string.Empty;
            if(id>0)
            {
                string token = string.Empty;
                var claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
                var userTokenClaim = claimsIdentity.Claims.SingleOrDefault(c => c.Type == "Token");
                if (userTokenClaim != null)
                {
                    token = userTokenClaim.Value;
                }
                data = await ApiClientFactory.Instance.DeletePackage(token, id);

                //if (data.statusCode != null && Convert.ToInt32(data.statusCode)==200 )
                //{
                //    var model = await ApiClientFactory.Instance.GetPackages(token);

                //    viewFromCurrentController = await this.RenderViewToStringAsync("PartialGetPackages", model.responseObject);
                //}
                objResponse.StatusCode = Convert.ToInt32(data.statusCode);
                //objResponse.Html = viewFromCurrentController;
                if (Convert.ToInt32(data.statusCode) == 200)
                {
                    await TrackingInfo.TrackInfo(token, EmployeeId, ControllerContext.ActionDescriptor.ControllerName, id.ToString(),id, "Delete");
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
      
        //public async Task<string> VoidTrackInfo(string token,string title,int id, string type)
        //{
        //    var usr = await GetCurrentUserAsync();
        //    var model = new TrackingInfoModel()
        //    {
        //        EmployeeId = usr?.Id,
        //        ModuleName = ControllerContext.ActionDescriptor.ControllerName,
        //        WorkDescription = type + ":-"+"Id:-"+id + ", Title:-" + title
        //    };
        //   var response=await ApiClientFactory.Instance.SaveTackingInfo(token, model);
        //   return Convert.ToString(response.StatusCode);
        //}
    }
}
