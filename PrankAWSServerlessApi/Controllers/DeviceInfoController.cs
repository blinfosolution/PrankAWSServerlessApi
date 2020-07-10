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
    public class DeviceInfoController : BaseApiController
    {
        public DeviceInfoController(DataContext dataContext, IConfiguration config, FileSettings fileSettings, IWebHostEnvironment env) : base(dataContext, config, fileSettings, env)
        {

        }

        [HttpPost]
        [Route("ValidateDeviceInfoRequest")]
        public async Task<ActionResult<ResponseModel>> ValidateDeviceInfoRequest([FromBody] DeviceInfoModel deviceInfo)
        {
            var model = new ResponseModel();
            var result = await _dataContext.ValidateDeviceInfoRequest(deviceInfo);

            if (!result.Status)
            {
                model.StatusCode = 404;
                model.Message = result.Message;
            }
            else
            {
                model.StatusCode = 200;
                deviceInfo.DeviceId = result.Id;
                model.ResponseObject = deviceInfo;
            }

            return model;
        }

        [HttpGet]
        [Route("GetDeviceInfoByKey")]
        public async Task<ActionResult<ResponseModel>> GetDeviceInfoByKey(string deviceKey, string search, string sortCol, string sortDir, int skip, int take)
        {
            try
            {
                deviceKey = string.IsNullOrEmpty(deviceKey) ? string.Empty : deviceKey;
                search = string.IsNullOrEmpty(search) ? string.Empty : search;
                sortCol = string.IsNullOrEmpty(sortCol) ? "DeviceId" : sortCol;
                sortDir = string.IsNullOrEmpty(sortDir) ? "desc" : sortDir;

                var model = new ResponseModel();
                var objDeviceList = await _dataContext.GetDeviceInfoByKey(deviceKey, search, sortCol, sortDir, skip, take).ToListAsync();

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
        [Route("AddDeviceInfo")]
        public async Task<ActionResult<ResponseModel>> AddDeviceInfo([FromBody] DeviceInfoModel deviceInfo)
        {
            var model = new ResponseModel();
            var result = await _dataContext.AddDeviceInfo(deviceInfo);

            if (!result.Status)
            {
                model.StatusCode = 404;
                model.Message = result.Message;
            }
            else
            {
                model.StatusCode = 200;
                var objDeviceList = await _dataContext.GetDeviceInfoByKey(string.Empty, string.Empty, string.Empty,string.Empty, 0, 1000).ToListAsync();
                if (objDeviceList != null)
                {
                    model.ResponseObject = objDeviceList;
                }
            }
            return model;
        }

        [HttpPost]
        [Route("UpdateDeviceInfo")]
        public async Task<ActionResult<ResponseModel>> UpdateDeviceInfo([FromBody] DeviceInfoModel deviceInfo)
        {
            var model = new ResponseModel();
            var result = await _dataContext.UpdateDeviceInfo(deviceInfo);

            if (!result.Status)
            {
                model.StatusCode = 404;
                model.Message = result.Message;
            }
            else
            {
                model.StatusCode = 200;
                var objDeviceList = await _dataContext.GetDeviceInfoByKey(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1000).ToListAsync();
                if (objDeviceList != null)
                {
                    model.ResponseObject = objDeviceList;
                }
            }
            return model;
        }

        [HttpGet]
        [Route("UpdateStatusDeviceInfo/{deviceId}/{isActive}")]
        public async Task<ActionResult<ResponseModel>> UpdateStatusDeviceInfo(int deviceId, bool isActive)
        {
            var model = new ResponseModel();
            var result = await _dataContext.UpdateStatusDeviceInfo(deviceId, isActive);

            if (!result.Status)
            {
                model.StatusCode = 404;
                model.Message = result.Message;
            }
            else
            {
                model.StatusCode = 200;
                var objDeviceList = await _dataContext.GetDeviceInfoByKey(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1000).ToListAsync();
                if (objDeviceList != null)
                {
                    model.ResponseObject = objDeviceList;
                }
            }
            return model;
        }

        [HttpDelete]
        [Route("DeleteDeviceInfo/{deviceId}")]
        public async Task<ActionResult<ResponseModel>> DeleteDeviceInfo(int deviceId)
        {
            var model = new ResponseModel();
            var result = await _dataContext.DeleteDeviceInfo(deviceId);

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