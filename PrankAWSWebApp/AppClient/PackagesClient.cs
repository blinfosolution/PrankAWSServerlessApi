using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using Prank.Model;
using PrankAWSWebApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PrankAWSWebApp.AppClient
{
    public partial class ApiClient
    {
        public async Task<PackagesDataModel> GetPackages(FilterModel filter)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "api/Packages/GetPackagesListByRequest"), "sortCol=" + filter.SortCol + "&sortDir=" + filter.SortDir + "&skip=" + filter.Skip + "&take=" + filter.Take);
            return await GetAsync<PackagesDataModel>(requestUrl, filter.Token);

           
            return null;
        }
        public async Task<PackagesDataModel> SavePackages(string token, PackagesModel packagesModel)
        {


            string attachmentstring = JsonConvert.SerializeObject(packagesModel);
            var httpContent = new StringContent(attachmentstring, Encoding.UTF8, "application/json");
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "api/Packages/AddPackages"));
            return await PostAsync<PackagesDataModel>(requestUrl, httpContent);
        }
        public async Task<PackagesDataModel> UpdatePackages(string token, PackagesModel packagesModel)
        {
            string attachmentstring = JsonConvert.SerializeObject(packagesModel);
            var httpContent = new StringContent(attachmentstring, Encoding.UTF8, "application/json");
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "api/Packages/UpdatePackages"));
            return await PostAsync<PackagesDataModel>(requestUrl, httpContent);
        }
        public async Task<PackagesModelViewModel> GetPackageById(string token, int id)
        {
            string sortCol = string.Empty;
            string sortDir = string.Empty;
            int skip = 0;
            int take = 10;
            var model = new PackagesModelViewModel();
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "api/Packages/GetPackagesListByRequest"), "sortCol=" + sortCol + "&sortDir=" + sortDir + "&skip=" + skip + "&take=" + take);
            //  "api/Packages/GetPackagesListByRequest"), "packageId=" + id);

            var packageModel = await GetAsync<PackagesDataModel>(requestUrl, token);
            if (packageModel != null && packageModel.responseObject.Any())
            {
                model = packageModel.responseObject.FirstOrDefault();
            }
            return model;
        }
        public async Task<PackagesDataModel> DeletePackage(string token, int id)
        {
            var model = new PackagesModelViewModel();
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
              "api/Packages/DeletePackages/" + id), "");

            return await DeleteAsync<PackagesDataModel>(requestUrl, token);
        }



    }
}
