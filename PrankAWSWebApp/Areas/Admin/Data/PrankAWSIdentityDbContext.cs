using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PrankAWSWebApp.Areas.Admin.Data
{
    public class PrankAWSIdentityDbContext :IdentityDbContext<PrankIdentityUser, PrankIdentityRole, string>
    {
       

        public PrankAWSIdentityDbContext
           (DbContextOptions<PrankAWSIdentityDbContext> options)
            : base(options)
        {
            //nothing here
        }

	}
}