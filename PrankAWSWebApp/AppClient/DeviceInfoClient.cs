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
        public async Task<List<DeviceInfoListModel>> GetDeviceInfo(FilterModel filter)
        {
            try
            {
                var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                    "api/DeviceInfo/GetDeviceInfoByKey"), "search="+ filter.Search + "&sortCol=" + filter.SortCol + "&sortDir=" + filter.SortDir + "&skip=" + filter.Skip + "&take=" + filter.Take );
                var result = await GetAsync<ResponseModel>(requestUrl, filter.Token);
                if (result.ResponseObject != null)
                    return JsonConvert.DeserializeObject<List<DeviceInfoListModel>>(result.ResponseObject.ToString());
                return null;
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public async Task<ResponseModel> ChangeStatusDeviceInfo(string token, int deviceId, bool isActive)
        {
            try
            {              
                var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                    "api/DeviceInfo/UpdateStatusDeviceInfo/")+deviceId+"/"+ isActive);
              return  await GetAsync<ResponseModel>(requestUrl, token);
             }
            catch (Exception e)
            {

                throw;
            }
        }


    }
}
