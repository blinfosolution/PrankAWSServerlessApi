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
   //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PrankInfoController : BaseApiController
    {

        public PrankInfoController(DataContext dataContext, IConfiguration config, FileSettings fileSettings, IWebHostEnvironment env) : base(dataContext, config, fileSettings, env)
        {

        }

        [HttpGet]
        [Route("GetPrankInfoById")]
        public async Task<ActionResult<ResponseModel>> GetPrankInfoById(int prankId)
        {
            var model = new ResponseModel();
            var objPrankList = await _dataContext.GetPrankInfoById(prankId).ToListAsync();

            if (objPrankList != null)
            {
                model.ResponseObject = objPrankList;
                model.StatusCode = 200;
            }
            else
            {
                model.StatusCode = 404;
            }

            return model;
        }

        [HttpGet]
        [Route("GetAllPrankListByRequest")]
        public async Task<ActionResult<ResponseModel>> GetAllPrankListByRequest(string prankGroup, string search,int skip, int take,string sortCol,string sortDir)
        {

            search = string.IsNullOrEmpty(search) ? string.Empty : search;

            sortCol = string.IsNullOrEmpty(sortCol) ? "PrankId" : sortCol;
            sortDir = string.IsNullOrEmpty(sortDir) ? "desc" : sortDir;
            var model = new ResponseModel();
            var objPrankList = await _dataContext.GetAllPranksListByPram(string.Empty, search,skip, take,sortCol,sortDir).ToListAsync();

            if (objPrankList != null)
            {
                model.ResponseObject = objPrankList;
                model.StatusCode = 200;
            }
            else
            {
                model.StatusCode = 404;
            }

            return model;
        }


        [HttpGet]
        [Route("GetPrankListByRequest")]
        public async Task<ActionResult<ResponseModel>> GetPrankListByRequest(string prankGroup, int skip, int take)
        {
            var model = new ResponseModel();
            var objPrankList = await _dataContext.GetPranksListByPram(string.Empty, skip, take).ToListAsync();

            if (objPrankList != null)
            {
                model.ResponseObject = objPrankList;
                model.StatusCode = 200;
            }
            else
            {
                model.StatusCode = 404;
            }

            return model;
        }

        [HttpPost]
        [Route("AddPrankInfo")]
        public async Task<ActionResult<ResponseModel>> AddPrankInfo([FromBody] PrankInfoModel prankInfo)
        {
            var model = new ResponseModel();
            var result = await _dataContext.AddPrankInfo(prankInfo);

            if (!result.Status)
            {
                model.StatusCode = 404;
                model.Message = result.Message;
            }
            else
            {
                model.StatusCode = 200;
                var objPrankList = await _dataContext.GetAllPranksListByPram(string.Empty,string.Empty, 0, 1000, string.Empty, string.Empty).ToListAsync();
                if (objPrankList != null)
                {
                    model.ResponseObject = objPrankList;
                }
            }

            return model;
        }

        [HttpPost]
        [Route("UpdatePrankInfo")]
        public async Task<ActionResult<ResponseModel>> UpdatePrankInfo([FromBody] PrankInfoModel prankInfo)
        {
            var model = new ResponseModel();
            var result = await _dataContext.UpdatePrankInfo(prankInfo);

            if (!result.Status)
            {
                model.StatusCode = 404;
                model.Message = result.Message;
            }
            else
            {
                model.StatusCode = 200;
                var objPrankList = await _dataContext.GetAllPranksListByPram(string.Empty, string.Empty, 0, 10,string.Empty,string.Empty).ToListAsync();
                if (objPrankList != null)
                {
                    model.ResponseObject = objPrankList;
                }
            }

            return model;
        }

        [HttpDelete]
        [Route("DeletePrankInfo/{PrankId}")]
        public async Task<ActionResult<ResponseModel>> DeletePrankInfo(int PrankId)
        {
            var model = new ResponseModel();
            var result = await _dataContext.DeletePrankInfo(PrankId);

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
