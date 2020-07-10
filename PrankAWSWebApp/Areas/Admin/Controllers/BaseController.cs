using Microsoft.Extensions.Options;
using PrankAWSWebApp.Utility;
using Microsoft.AspNetCore.Identity;
using PrankAWSWebApp.Areas.Admin.Data;
using Microsoft.AspNetCore.Mvc;
using PrankAWSWebApp.Models;
namespace PrankAWSWebApp.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {

        private readonly IOptions<MySettingsModel> appSettings;
        private readonly SignInManager<PrankIdentityUser> loginManager;
        private readonly UserManager<PrankIdentityUser> userManager;


        public BaseController(IOptions<MySettingsModel> app, SignInManager<PrankIdentityUser> loginManager, UserManager<PrankIdentityUser> userManager)
        {
            appSettings = app;
            ApplicationSettings.WebApiUrl = appSettings.Value.WebApiBaseUrl;
            this.loginManager = loginManager;
            this.userManager = userManager;

        }

       
    }
}
