using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Prank.DAL;
using Prank.Model;
using PrankAWSServerlessApi.ApiSettings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using PrankAWSServerlessApi.Common;

namespace PrankAWSServerlessApi.Controllers
{  
    [ApiController]
    public class LoginController : BaseApiController
    {
        public LoginController(DataContext dataContext, IConfiguration config, FileSettings fileSettings, IWebHostEnvironment env) : base(dataContext, config, fileSettings, env)
        {

        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/GenerateToken")]
        public IActionResult Login(string randomString)
        {
            IActionResult response = Unauthorized();

            if (!string.IsNullOrEmpty(randomString))
            {
                var tokenString = TokenManager.GenerateToken(randomString, _IConfiguration["Jwt:Key"].ToString(), _IConfiguration["Jwt:Issuer"].ToString(), _IConfiguration["Jwt:Audience"].ToString());
                response = Ok(new
                {
                    token = tokenString,
                    userDetails = randomString,
                });
            }
            return response;
        }

        //MemberModel AuthenticateUser(MemberModel loginCredentials)
        //{
        //    //User user = appUsers.SingleOrDefault(x => x.UserName == loginCredentials.UserName && x.Password == loginCredentials.Password);

        //    return loginCredentials;
        //}
    }
}
