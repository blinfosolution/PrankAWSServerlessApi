using System;
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
    public class TrackingInfoController : BaseApiController
    {
        public TrackingInfoController(DataContext dataContext, IConfiguration config, FileSettings fileSettings, IWebHostEnvironment env) : base(dataContext, config, fileSettings, env)
        {

        }
        [HttpGet]
        [Route("GetAllTrackingInfoListByRequest")]
        public async Task<ActionResult<ResponseModel>> GetAllTrackingInfoListByRequest( string search, string sortCol, string sortDir, int skip, int take)
        {
            try
            {
                search = string.IsNullOrEmpty(search) ? string.Empty : search;
                sortCol = string.IsNullOrEmpty(sortCol) ? "TrackingId" : sortCol;
                sortDir = string.IsNullOrEmpty(sortDir) ? "desc" : sortDir;

                var model = new ResponseModel();
                var objDeviceList = await _dataContext.GetAllTrackingInfoListByRequest( search, sortCol, sortDir, skip, take).ToListAsync();

                if (objDeviceList != null)
                {
                    model.ResponseObject = objDeviceList;
                    model.StatusCode = 200;
                }
                else
                {
                    model.StatusCode = 404;
                }
                return model;
            }
            catch (System.Exception e)
            {

                throw;
            }

        }

        [HttpPost]
        [Route("AddTrackingInfo")]
        public async Task<ActionResult<ResponseModel>> AddTrackingInfo([FromBody] TrackingInfoModel trackingInfo)
        {
            var model = new ResponseModel();
            var result = await _dataContext.AddTrackingInfo(trackingInfo);

            if (!result.Status)
            {
                model.StatusCode = 404;
                model.Message = result.Message;
            }
            else
            {
                model.StatusCode = 200;
                var objtrackingList = await _dataContext.GetAllTrackingInfoListByRequest( string.Empty, string.Empty, string.Empty, 0, 1000).ToListAsync();
                if (objtrackingList != null)
                {
                    model.ResponseObject = objtrackingList;
                }
            }
            return model;
        }

    }
}
