using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PrankAWSWebApp.Controllers
{
    public class BaseController : Controller
    {


        public bool CheckUserPermission(int moduleId, string permissionFor)
        {
            var flag = false;
            if (User != null)
            {
                var userIdentity = (ClaimsPrincipal)Thread.CurrentPrincipal;

                var identity = (ClaimsIdentity)User.Identity;
                var objRoles = identity.Claims.Where(c => c.Type == "UserName").ToList();
                if (objRoles != null && objRoles.Any())
                {
                    string roleId = string.Empty;

                    if (objRoles.Count > 0)
                    {
                        roleId = objRoles[0]?.Value;
                    }
                }
            }

            return flag;
        }



    }
}
