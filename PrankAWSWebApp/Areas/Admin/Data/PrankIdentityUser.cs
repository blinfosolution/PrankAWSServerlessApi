using Microsoft.AspNetCore.Identity;
using Prank.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrankAWSWebApp.Areas.Admin.Data
{
    public class PrankIdentityUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }
        [NotMapped]
        public string Token { get; internal set; }
    }
}
