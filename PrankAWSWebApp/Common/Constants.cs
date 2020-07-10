using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrankAWSWebApp.Common
{
    public static class Constants
    {
        //internal const string UserSession = "UserSession";

        //internal const string Issuer = "https://yoursite.com";

        //internal static class UserRoles
        public static class UserRoles
        {
            //internal const string Admin = "Admin";
            //internal const string Director = "Director";
            //internal const string Supervisor = "Supervisor";
            //internal const string Employee = "Employee";
            public const string SuperAdmin = "SuperAdmin";
            public const string Admin = "Admin";
            public const string Director = "Director";
            public const string Supervisor = "Supervisor";
            public const string Employee = "Employee";
        }


        internal static class Permissions
        {
            internal const string Create = "Create";
            internal const string Update = "Update";
            internal const string Delete = "Delete";
            internal const string List = "List";
        }
    }
}
