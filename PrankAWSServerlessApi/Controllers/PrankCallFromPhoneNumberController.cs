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
    public class PrankCallFromPhoneNumberController : BaseApiController
    {
        public PrankCallFromPhoneNumberController(DataContext dataContext, IConfiguration config, FileSettings fileSettings, IWebHostEnvironment env) : base(dataContext, config, fileSettings, env)
        {
        }

    
        [HttpGet]
        [Route("GetPrankCallFromPhoneNumberList")]
        public async Task<ActionResult<ResponseModel>> GetPrankCallFromPhoneNumberList(string PrankCallFromPhoneNumberGroup, int PrankCallFromId, string search, int skip, int take, string sortCol, string sortDir)
        {
            search = string.IsNullOrEmpty(search) ? string.Empty : search;

            sortCol = string.IsNullOrEmpty(sortCol) ? "PrankCallFromId" : sortCol;
            sortDir = string.IsNullOrEmpty(sortDir) ? "desc" : sortDir;
            var model = new ResponseModel();
            var objPrankCallFromPhoneNumberList = await _dataContext.GetPrankCallFromPhoneNumberByPram(string.Empty, PrankCallFromId, search, skip, take, sortCol, sortDir).ToListAsync();

            if (objPrankCallFromPhoneNumberList != null)
            {
                model.ResponseObject = objPrankCallFromPhoneNumberList;
                model.StatusCode = 200;
            }
            else
            {
                model.StatusCode = 404;
            }

            return model;
        }

        [HttpPost]
        [Route("AddPrankCallFromPhoneNumber")]
        public async Task<ActionResult<ResponseModel>> AddPrankCallFromPhoneNumberInfo([FromBody] PrankCallFromPhoneNumberModel PrankCallFromPhoneNumberInfo)
        {
            var model = new ResponseModel();
            var result = await _dataContext.AddPrankCallFromPhoneNumber(PrankCallFromPhoneNumberInfo);

            if (!result.Status)
            {
                model.StatusCode = 404;
                model.Message = result.Message;
            }
            else
            {
                model.StatusCode = 200;
                var objPrankCallFromPhoneNumberList = await _dataContext.GetPrankCallFromPhoneNumberByPram(string.Empty, 0,string.Empty,0,1000000,string.Empty,string.Empty).ToListAsync();
                if (objPrankCallFromPhoneNumberList != null)
                {
                    model.ResponseObject = objPrankCallFromPhoneNumberList;
                }
            }

            return model;
        }

        [HttpPost]
        [Route("UpdatePrankCallFromPhoneNumber")]
        public async Task<ActionResult<ResponseModel>> UpdatePrankCallFromPhoneNumberInfo([FromBody] PrankCallFromPhoneNumberModel prankCallFromPhoneNumber)
        {
            var model = new ResponseModel();
            var result = await _dataContext.UpdatePrankCallFromPhoneNumber(prankCallFromPhoneNumber);

            if (!result.Status)
            {
                model.StatusCode = 404;
                model.Message = result.Message;
            }
            else
            {
                model.StatusCode = 200;
                var objPrankCallFromPhoneNumberList = await _dataContext.GetPrankCallFromPhoneNumberByPram(string.Empty, 0,string.Empty, 0, 1000000, string.Empty, string.Empty).ToListAsync();
                if (objPrankCallFromPhoneNumberList != null)
                {
                    model.ResponseObject = objPrankCallFromPhoneNumberList;
                }
            }

            return model;
        }

        [HttpDelete]
        [Route("DeletePrankCallFromPhoneNumber/{PrankCallFromPhoneNumberId}")]
        public async Task<ActionResult<ResponseModel>> DeletePrankCallFromPhoneNumber(int PrankCallFromPhoneNumberId)
        {
            var model = new ResponseModel();
            var result = await _dataContext.DeletePrankCallFromPhoneNumber(PrankCallFromPhoneNumberId);

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


        [HttpGet]
        [Route("UpdateIsDefaultStatusFromPhoneNumber/{PrankCallFromId}/{IsDefault}")]
        public async Task<ActionResult<ResponseModel>> UpdateIsDefaultStatusFromPhoneNumber(int PrankCallFromId, bool IsDefault)
        {
            var model = new ResponseModel();
            var result = await _dataContext.UpdateIsDefaultStatusFromPhoneNumber(PrankCallFromId, IsDefault);

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

    }
}
