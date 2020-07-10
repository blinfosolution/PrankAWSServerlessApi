using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Prank.DAL;
using Prank.Model;
using PrankAWSServerlessApi.ApiSettings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace PrankAWSServerlessApi.Controllers
{
   [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController : BaseApiController
    {
        public PackagesController(DataContext dataContext, IConfiguration config, FileSettings fileSettings, IWebHostEnvironment env) :
            base(dataContext, config, fileSettings, env)
        {

        }

        [HttpGet]
        [Route("GetPackagesListByRequest")]
        public async Task<ActionResult<ResponseModel>> GetPackagesListByRequest( int packageId,  string sortCol, string sortDir, int skip, int take)
        {
          
            sortCol = string.IsNullOrEmpty(sortCol) ? "PackageId" : sortCol;
            sortDir = string.IsNullOrEmpty(sortDir) ? "desc" : sortDir;

            var model = new ResponseModel();
            var objPackagesList = await _dataContext.GetPackagesByPram( packageId,sortCol,sortDir,skip,take).ToListAsync();

            if (objPackagesList != null)
            {
                model.ResponseObject = objPackagesList;
                model.StatusCode = 200;
            }
            else
            {
                model.StatusCode = 404;
            }

            return model;
        }

        [HttpPost]
        [Route("AddPackages")]
        public async Task<ActionResult<ResponseModel>> AddPackages([FromBody] PackagesModel Packages)
        {
            var model = new ResponseModel();
            var result = await _dataContext.AddPackages(Packages);

            if (!result.Status)
            {
                model.StatusCode = 404;
                model.Message = result.Message;
            }
            else
            {
                model.StatusCode = 200;
                var objPackagesList = await _dataContext.GetPackagesByPram(0, string.Empty,string.Empty,0,10000).ToListAsync();
                if (objPackagesList != null)
                {
                    model.ResponseObject = objPackagesList;
                }
            }

            return model;
        }

        [HttpPost]
        [Route("UpdatePackages")]
        public async Task<ActionResult<ResponseModel>> UpdatePackages([FromBody] PackagesModel Packages)
        {
            var model = new ResponseModel();
            var result = await _dataContext.UpdatePackages(Packages);

            if (!result.Status)
            {
                model.StatusCode = 404;
                model.Message = result.Message;
            }
            else
            {
                model.StatusCode = 200;
                var objPackagesList = await _dataContext.GetPackagesByPram(0,string.Empty,string.Empty,0,10000).ToListAsync();
                if (objPackagesList != null)
                {
                    model.ResponseObject = objPackagesList;
                }
            }

            return model;
        }

        [HttpDelete]
        [Route("DeletePackages/{PackagesId}")]
        public async Task<ActionResult<ResponseModel>> DeletePackages(int PackagesId)
        {
            var model = new ResponseModel();
            var result = await _dataContext.DeletePackages(PackagesId);

            if (!result.Status)
            {
                model.StatusCode = 404;
                model.Message = result.Message;
            }
            else
            {
                model.StatusCode = 200;
            }

            return model;
        }
    }
}
