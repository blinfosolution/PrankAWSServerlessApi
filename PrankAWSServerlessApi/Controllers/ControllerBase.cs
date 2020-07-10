using Microsoft.AspNetCore.Mvc;
using Prank.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Filters;
using PrankAWSServerlessApi.Common;

namespace PrankAWSServerlessApi.Controllers
{
    public class ControllerBase : Controller
    {
        protected ServiceUser ServiceUser { get; set; }
        protected IConfiguration _config;

        public ControllerBase(IConfiguration config)
        {
            _config = config;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ControllerContext
                .HttpContext
                .Items
                .TryGetValue(
                    Constants.HttpContextServiceUserItemKey,
                    out object serviceUser);
            ServiceUser = serviceUser as ServiceUser;

            if (serviceUser != null && !string.IsNullOrEmpty(ServiceUser.Token))
            {
               // string tokenUsername = TokenManager.ValidateToken(ServiceUser.Token, _config["Jwt:Key"].ToString());
            }

            base.OnActionExecuting(context);
        }
    }
}
