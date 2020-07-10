using Newtonsoft.Json;
using Prank.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PrankAWSWebApp.AppClient
{
    public partial class ApiClient
    {
        #region List
     
        public async Task<List<PrankCallFromPhoneNumberLstModel>> GetPrankCallFromPhoneNoLst(string token, int fromId, int skip, int take, string search, string sortCol, string sortDir)
        {
            try
            {
                var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                    "api/PrankCallFromPhoneNumber/GetPrankCallFromPhoneNumberList"), "PrankCallFromId=" + fromId + "&skip=" + skip + "&take=" + take + "&search=" + search + "&sortCol=" + sortCol + "&sortDir=" + sortDir);
                var result = await GetAsync<ResponseModel>(requestUrl, token);
                if (result.ResponseObject != null)
                    return JsonConvert.DeserializeObject<List<PrankCallFromPhoneNumberLstModel>>(result.ResponseObject.ToString());
                return null;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion

        #region SaveUpdate 
        public async Task<ResponseModel> SavePrankCallFromPhoneNo(PrankCallFromPhoneNumberModel model)
        {
            try
            {
                string attachmentstring = JsonConvert.SerializeObject(model);
                var httpContent = new StringContent(attachmentstring, Encoding.UTF8, "application/json");
                var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                    "api/PrankCallFromPhoneNumber/AddPrankCallFromPhoneNumber"));
                return await PostAsync<ResponseModel>(requestUrl, httpContent);
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public async Task<ResponseModel> UpdatePrankCallFromPhoneNo(PrankCallFromPhoneNumberModel model)
        {
            try
            {
                string attachmentstring = JsonConvert.SerializeObject(model);
                var httpContent = new StringContent(attachmentstring, Encoding.UTF8, "application/json");
                var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                    "api/PrankCallFromPhoneNumber/UpdatePrankCallFromPhoneNumber"));
                return await PostAsync<ResponseModel>(requestUrl, httpContent);
            }
            catch (Exception e)
            {

                throw;
            }
        }
        #endregion

        #region Delete
        public async Task<bool> DeletePrankCallFromPhoneNo(string token, int id)
        {
            try
            {
                var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                     "api/PrankCallFromPhoneNumber/DeletePrankCallFromPhoneNumber/") + id);
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

        public async Task<ResponseModel> ChangeStatusPrankCallFromPhoneNumber(string token, int fromId, bool isDefault)
        {
            try
            {
                var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                    "api/PrankCallFromPhoneNumber/UpdateIsDefaultStatusFromPhoneNumber/") + fromId + "/" + isDefault);
                return await GetAsync<ResponseModel>(requestUrl, token);
            }
            catch (Exception e)
            {

                throw;
            }
        }
        #endregion
    }
}
