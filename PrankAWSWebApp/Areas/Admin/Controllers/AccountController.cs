using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Prank.Model.Account;
using PrankAWSWebApp.Areas.Admin.Data;
using Newtonsoft.Json;
using System.Linq;
using System.Security.Claims;
using System;

namespace PrankAWSWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class AccountController : Controller
    {
        IConfiguration _iconfiguration;

        private readonly UserManager<PrankIdentityUser> userManager;
        private readonly SignInManager<PrankIdentityUser> loginManager;
        private readonly RoleManager<PrankIdentityRole> roleManager;

        private readonly CustomClaimsCookieSignInHelper<PrankIdentityUser> _customClaimsCookieSignInHelper;
        public AccountController(CustomClaimsCookieSignInHelper<PrankIdentityUser> claimManager, UserManager<PrankIdentityUser> userManager, SignInManager<PrankIdentityUser> loginManager, RoleManager<PrankIdentityRole> roleManager, IConfiguration iconfiguration)
        {
            this.userManager = userManager;
            this.loginManager = loginManager;
            this.roleManager = roleManager;
            _iconfiguration = iconfiguration;
            _customClaimsCookieSignInHelper = claimManager;
        }

        public object JwtClaimTypes { get; private set; }

        public async Task<IActionResult> Login()
        {
            //var user = new PrankIdentityUser { UserName = "Admin@gmail.com", Email = "Admin@gmail.com", FirstName = "Admin", LastName = "Sharma", IsAdmin = true };
            //var result = await userManager.CreateAsync(user, "Admin@123");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Login(LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = loginManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false).Result;
                    if (result != null && result.Succeeded)
                    {
                        #region Generate Token
                        var task = Task.Run(async () => await GetToken(model));
                        var token = task.Result;  
                        #endregion
                        if (token != null)
                        {
                            var user = await userManager.FindByEmailAsync(model.Email);
                            var userroles = await userManager.GetRolesAsync(user);
                            List<Claim> LiecenceClaims = new List<Claim>();

                            if (userroles.Count > 0)
                            {
                                foreach (var role in userroles)
                                {
                                    LiecenceClaims.Add(
                                        new Claim(ClaimTypes.Role, role)
                                      //new Claim("RoleName", role)
                                      );
                                }
                            }
                            //var liecenceIdentity = new ClaimsIdentity(LiecenceClaims, "UserIdentity");
                            //var claimsPrincipal = new ClaimsPrincipal(liecenceIdentity);
                            //await userManager.AddClaimsAsync(user, LiecenceClaims);

                            var customClaims = new List<Claim> { new Claim("Token", token.token), new Claim("UserName", user.UserName), new Claim("Email", user.Email) };
                            customClaims.AddRange(LiecenceClaims);
                            await _customClaimsCookieSignInHelper.SignInUserAsync(user, model.RememberMe, customClaims);

                            List<string> lstString = new List<string>
                    {
                        "/Admin/DashBoard/Index"
                    };

                            return new JsonResult(lstString);
                        }
                    }
                    ModelState.AddModelError("", "Invalid login!");
                }
            }
            catch (System.Exception e)
            {
                throw;
            }
            return new JsonResult(new List<string>());
        }
        public IActionResult LogOff()
        {
            loginManager.SignOutAsync().Wait();
            return RedirectToAction("Login", "Account");
        }
        public async Task<AccessToken> GetToken(LoginViewModel model)
        {
            var token = new AccessToken();
            using (var client = new HttpClient())
            {
                //var token = new AccessToken();
                var tokenServiceUrl = _iconfiguration["BaseApiUrl"] + _iconfiguration["GetTokenApi"] + model.Email
                    ;
                var rquestParams = new List<KeyValuePair<string, string>>
                {
                    //   new KeyValuePair<string, string>("grant_type", "password"),
                    //   new KeyValuePair<string, string>("randomString", model.Email),
                    //  new KeyValuePair<string, string>("password", model.Password)
                };

                var rquestParamsFormUrlEncodedContent = new FormUrlEncodedContent(rquestParams);
                var tokenServiceResponse = await client.PostAsync(tokenServiceUrl, rquestParamsFormUrlEncodedContent);
                var responseStr = await tokenServiceResponse.Content.ReadAsStringAsync();
                var responseCode = tokenServiceResponse.StatusCode;

                if (responseCode == System.Net.HttpStatusCode.OK)
                {
                    token = JsonConvert.DeserializeObject<AccessToken>(responseStr);
                }
            }
            return token;
        }
    }


    public class AccessToken
    {
        public string token { get; set; }
        public string userDetails { get; set; }
        public string expires_in { get; set; }
        public string userName { get; set; }
        public string issued { get; set; }
        public string expires { get; set; }

    }
}
