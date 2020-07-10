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

        #region List
        public async Task<List<PrankInfoListModel>> GetPrankInfo(string token, int skip, int take, string search, string sortCol, string sortDir)
        {
            try
            {
                var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                    "api/PrankInfo/GetAllPrankListByRequest"), "skip=" + skip + "&take=" + take + "&search=" + search + "&sortCol=" + sortCol + "&sortDir=" + sortDir);
                var result = await GetAsync<ResponseModel>(requestUrl, token);
                if (result.ResponseObject != null)
                    return JsonConvert.DeserializeObject<List<PrankInfoListModel>>(result.ResponseObject.ToString());
                return null;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public async Task<List<PrankInfoListModel>> GetPrankInfoById(string token, int prankId)
        {
            try
            {
                var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                    "api/PrankInfo/GetPrankInfoById"), "PrankId=" + prankId);
                var result = await GetAsync<ResponseModel>(requestUrl, token);
                if (result.ResponseObject != null)
                    return JsonConvert.DeserializeObject<List<PrankInfoListModel>>(result.ResponseObject.ToString());
                return null;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion

        #region SaveUpdate 
        public async Task<ResponseModel> SavePrankInfo(string token, PrankInfoModel model)
        {
            try
            {
                string attachmentstring = JsonConvert.SerializeObject(model);
                var httpContent = new StringContent(attachmentstring, Encoding.UTF8, "application/json");
                var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                    "api/PrankInfo/AddPrankInfo"));
                return await PostAsync<ResponseModel>(requestUrl, httpContent);
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public async Task<ResponseModel> UpdatePrankInfo(string token, PrankInfoModel model)
        {
            try
            {
                string attachmentstring =  JsonConvert.SerializeObject(model) ;
                var httpContent = new StringContent(attachmentstring, Encoding.UTF8, "application/json");
                var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                    "api/PrankInfo/UpdatePrankInfo"));
                return await PostAsync<ResponseModel>(requestUrl, httpContent);
            }
            catch (Exception e)
            {
                throw;
            }
        }
        #endregion

        #region Delete
        public async Task<bool> DeletePrankinfo(string token, int id)
        {
            try
            {
                var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                     "api/PrankInfo/DeletePrankInfo/") + id);
                var result = await DeleteAsync<ResponseModel>(requestUrl, token);
                if (result.StatusCode == 200)
                    return true;
                return false;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        #endregion
    }
}
