using Prank.Model;
using PrankAWSWebApp.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrankAWSWebApp.Common
{
    public static class TrackingInfo
    {
        public static async Task<string> TrackInfo(string token,string employeeId,string controllerName, string title, int id, string type)
        {

            var model = new TrackingInfoModel()
            {
                EmployeeId = employeeId,
                ModuleName = controllerName,// ControllerContext.ActionDescriptor.ControllerName,
                WorkDescription = type + ":-" + "Id:-" + id + ", Title:-" + title
            };
            var response = await ApiClientFactory.Instance.SaveTackingInfo(token, model);
            return Convert.ToString(response.StatusCode);
        }
    }
}
