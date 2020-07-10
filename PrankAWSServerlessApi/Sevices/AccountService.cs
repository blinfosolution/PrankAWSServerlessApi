using Microsoft.AspNetCore.Http;
using Prank.DAL;
using Prank.Model;
using PrankAWSServerlessApi.Common;
using PrankAWSServerlessApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrankAWSServerlessApi.Sevices
{
    public class AccountService : ServiceBase
    {
        private readonly DataContext _dataContext;
        public AccountService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        //public MemberModel Authorize(LoginModel model)
        //{
        //    return _dataContext.MemberLogin(model).FirstOrDefault();
        //}
        public ServiceUser Login(HttpContext context, string login, string password)
        {
            context.Response.Cookies.Append(Constants.AuthorizationCookieKey, login);

            return new ServiceUser
            {
                Login = login
            };
        }
        public Result<ServiceUser> Verify(HttpContext context)
        {
            if (context.Request.Cookies[Constants.AuthorizationCookieKey] != null)
            {
                var legacyCookie = context.Request.Cookies[Constants.AuthorizationCookieKey].FromLegacyCookieString();

                var serviceUser = new ServiceUser();

                if (legacyCookie != null && legacyCookie.Any())
                {
                    serviceUser.Login = legacyCookie.FirstOrDefault(k => k.Key == "Login").Value;
                    serviceUser.Token = legacyCookie.FirstOrDefault(k => k.Key == "Token").Value;
                }
                else
                {
                    return Error<ServiceUser>();
                }

                return Ok(serviceUser);
            }
            else
            {
                return Error<ServiceUser>();
            }
        }
        public Result Logout(HttpContext context)
        {
            context.Response.Cookies.Delete(Constants.AuthorizationCookieKey);
            return Ok();
        }
    }
}
