using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PrankAWSWebApp.Areas.Admin.Data
{
    public class CustomClaimsCookieSignInHelper<PrankIdentityUser> where PrankIdentityUser : IdentityUser
    {
        private readonly SignInManager<PrankIdentityUser> _signInManager;

        public CustomClaimsCookieSignInHelper(SignInManager<PrankIdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task SignInUserAsync(PrankIdentityUser user, bool isPersistent, IEnumerable<Claim> customClaims)
        {
            var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
            if (customClaims != null && claimsPrincipal?.Identity is ClaimsIdentity claimsIdentity)
            {
                claimsIdentity.AddClaims(customClaims);
            }
            await _signInManager.Context.SignInAsync(IdentityConstants.ApplicationScheme,
                claimsPrincipal,
                new AuthenticationProperties { IsPersistent = isPersistent });
        }
    }
}
