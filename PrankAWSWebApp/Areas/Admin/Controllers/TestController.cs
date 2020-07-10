using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PrankAWSWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class TestController : Controller
    {
        public TestController()
        {

        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
