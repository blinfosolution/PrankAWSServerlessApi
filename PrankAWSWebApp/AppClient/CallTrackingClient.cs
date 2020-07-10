//using Amazon.Runtime.Internal;
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
        public async Task<List<PrankCallHistoryModel>> GetPrankCallHistoryLst( string token, int deviceId, int skip, int take)
        {
            try
            {
                string parameter = "skip=" + skip + "&take=" + take;
                if (deviceId > 0)
                {
                    parameter += "&deviceId=" + deviceId;
                }

                var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                    "api/PrankCallTracking/GetPrankCallHistory"), parameter);
                var result = await GetAsync<ResponseModel>(requestUrl, token);
                if (result.ResponseObject != null)
                    return JsonConvert.DeserializeObject<List<PrankCallHistoryModel>>(result.ResponseObject.ToString());
                return null;
            }
            catch (Exception e)
            {

                throw;
            }
        }       
        public async Task<bool> DeletePrankCallHistory(string token, int id)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
              "api/PrankCallTracking/DeletePrankCallHistory"), "trackingId=" + id);
            var result = await DeleteAsync<ResponseModel>(requestUrl, token);
            if (result.StatusCode == 200)
                return true;
            return false;
        }

    }
}
