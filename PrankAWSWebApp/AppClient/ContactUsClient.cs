using Amazon.S3.Model;
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

        public async Task<List<ContactUsEmailTrackModel>> GetContackUsLst(FilterModel filter)
        {
            try
            {
                var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                    "api/ContactUsEmailTrack/GetContactUsEmailTrackByRequest"), "emailTo=" + filter.Search + "&sortCol=" + filter.SortCol + "&sortDir=" + filter.SortDir + "&skip=" + filter.Skip + "&take=" + filter.Take);
                var result = await GetAsync<ResponseModel>(requestUrl, filter.Token);
                if (result.ResponseObject != null)
                    return JsonConvert.DeserializeObject<List<ContactUsEmailTrackModel>>(result.ResponseObject.ToString());
                return null;
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}
