using Newtonsoft.Json;
using Prank.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PrankAWSWebApp.AppClient
{
    public partial class ApiClient
    {
        #region SaveUpdate 
        public async Task<ResponseModel> SaveTackingInfo(string token, TrackingInfoModel model)
        {
            try
            {
                string attachmentstring = JsonConvert.SerializeObject(model);
                var httpContent = new StringContent(attachmentstring, Encoding.UTF8, "application/json");
                var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                    "api/TrackingInfo/AddTrackingInfo"));
                return await PostAsync<ResponseModel>(requestUrl, httpContent);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion
        #region List

        public async Task<List<TrackingInfoListModel>> GetTrackingInfo(FilterModel filter)
        {
            try
            {
                var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                    "api/TrackingInfo/GetAllTrackingInfoListByRequest"), "search=" + filter.Search + "&sortCol=" + filter.SortCol + "&sortDir=" + filter.SortDir + "&skip=" + filter.Skip + "&take=" + filter.Take);
                var result = await GetAsync<ResponseModel>(requestUrl, filter.Token);
                if (result.ResponseObject != null)
                    return JsonConvert.DeserializeObject<List<TrackingInfoListModel>>(result.ResponseObject.ToString());
                return null;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        #endregion

    }
}
