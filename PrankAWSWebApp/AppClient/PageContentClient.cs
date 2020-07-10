using Newtonsoft.Json;
using Prank.Model;
using PrankAWSWebApp.Models;
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
        public async Task<PageContentDataModel> GetPageContents(string token)
        {
            string PageContentGroup = "%22%22";
            int PageContentId = 0;
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "api/PageContent/GetPageContentListByRequest"), "pageContentKey=" + PageContentGroup+ "&pageContentId=" + PageContentId);
            var model= await GetAsync<PageContentDataModel>(requestUrl, token);
            return model;
        }

        public async Task<PageContentModelViewModel> GetPageContentId(string token, int id)
        {
            string PageContentGroup = "%22%22";
            int PageContentId = id;
            var model = new PageContentModelViewModel();
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
              "api/PageContent/GetPageContentListByRequest"), "pageContentKey=" + PageContentGroup + "&pageContentId=" + PageContentId);
            var pageContentModel = await GetAsync<PageContentDataModel>(requestUrl, token);
            if (pageContentModel != null && pageContentModel.responseObject.Any())
            {
                model = pageContentModel.responseObject.FirstOrDefault();
            }
            return model;
        }

        public async Task<PageContentDataModel> UpdatePageContent(string token, PageContentModel pageContentModel)
        {
            string attachmentstring = JsonConvert.SerializeObject(pageContentModel);
            var httpContent = new StringContent(attachmentstring, Encoding.UTF8, "application/json");
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "api/PageContent/UpdatePageContent"));
            return await PostAsync<PageContentDataModel>(requestUrl, httpContent);
        }
    }
}
