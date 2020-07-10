using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PrankAWSWebApp.Areas.Admin.Data
{
    public class PrankIdentityRole : IdentityRole
    {
        public PrankIdentityRole()
        {

        }
        public PrankIdentityRole(string roleName, string description)
        {
            Name = roleName;
            Description = description;
        }
        public string Description { get; set; }

     
    }
}
