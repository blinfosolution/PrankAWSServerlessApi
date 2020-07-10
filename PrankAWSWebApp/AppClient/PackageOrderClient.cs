using Newtonsoft.Json;
using Prank.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrankAWSWebApp.AppClient
{
    public partial class ApiClient
    {
        public async Task<List<PackageOrderModel>> GetPackageOrderLst(string token, int deviceId)
        {
            try
            {
                string parameter = string.Empty;
                if (deviceId > 0)
                {
                    parameter += "deviceId=" + deviceId;
                }

                var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                    "api/PackageOrder/GetPackageOrderInfoById"), parameter);
                var result = await GetAsync<ResponseModel>(requestUrl, token);
                if (result.ResponseObject != null)
                    return JsonConvert.DeserializeObject<List<PackageOrderModel>>(result.ResponseObject.ToString());
                return null;
            }
            catch (Exception e)
            {

                throw;
            }
        }



    }
}
