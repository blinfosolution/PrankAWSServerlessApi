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
        public async Task<List<ReferralInfoListModel>> GetReferralInfo(string token, int referralId, int skip, int take)
        {
            try
            {
                var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                    "api/ReferralInfo/GetAllReferralInfoListByRequest"), "referralId=" + referralId+"&skip=" + skip + "&take=" + take);
                var result = await GetAsync<ResponseModel>(requestUrl, token);
                if (result.ResponseObject != null)
                    return JsonConvert.DeserializeObject<List<ReferralInfoListModel>>(result.ResponseObject.ToString());
                return null;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        #endregion

        #region SaveUpdate 
        
        public async Task<ResponseModel> SaveReferralInfo( ReferralInfoModel model)
        {
            try
            {
                string attachmentstring = JsonConvert.SerializeObject(model);
                var httpContent = new StringContent(attachmentstring, Encoding.UTF8, "application/json");
                var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                    "api/ReferralInfo/AddReferralInfo"));
                return await PostAsync<ResponseModel>(requestUrl, httpContent);
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public async Task<ResponseModel> UpdateReferralInfo(ReferralInfoModel model)
        {
            try
            {
                string attachmentstring = JsonConvert.SerializeObject(model);
                var httpContent = new StringContent(attachmentstring, Encoding.UTF8, "application/json");
                var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                    "api/ReferralInfo/UpdatePrankReferalInfo"));
                return await PostAsync<ResponseModel>(requestUrl, httpContent);
            }
            catch (Exception e)
            {

                throw;
            }
        }
        #endregion

        #region Delete
        public async Task<bool> DeleteReferralInfo(string token, int id)
        {
            try
            {
                var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                     "api/ReferralInfo/DeletePrankReferalInfo/")+ id);
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
