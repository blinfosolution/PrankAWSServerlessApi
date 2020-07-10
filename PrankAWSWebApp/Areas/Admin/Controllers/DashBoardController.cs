using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PrankAWSWebApp.Areas.Admin.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace PrankAWSWebApp.Areas.Admin.Controllers
{
   // [Authorize]
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class DashBoardController : Controller
    {
        private readonly SignInManager<PrankIdentityUser> loginManager;
        private readonly IHostingEnvironment _env;
        public DashBoardController(SignInManager<PrankIdentityUser> loginManager, IHostingEnvironment env)
        {

            this.loginManager = loginManager;
            _env = env;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
