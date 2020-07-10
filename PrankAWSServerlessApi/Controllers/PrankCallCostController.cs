using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Prank.DAL;
using Prank.Model;
using PrankAWSServerlessApi.ApiSettings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace PrankAWSServerlessApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PrankCallCostController : BaseApiController
    {
        public PrankCallCostController(DataContext dataContext, IConfiguration config, FileSettings fileSettings, IWebHostEnvironment env) : base(dataContext, config, fileSettings, env)
        {
        }

        [HttpGet]
        [Route("GetPrankCallCostListByRequest")]
        public async Task<ActionResult<ResponseModel>> GetPrankCallCostListByRequest(string CountryPrefix)
        {
            var model = new ResponseModel();
            var objPrankCostLst = await _dataContext.GetPrankCallCostListByPram(CountryPrefix).ToListAsync();

            if (objPrankCostLst != null)
            {
                model.ResponseObject = objPrankCostLst;
                model.StatusCode = 200;
            }
            else
            {
                model.StatusCode = 200;
            }
            return model;
        }

        [HttpPost]
        [Route("AddPrankCallCost")]
        public async Task<ActionResult<ResponseModel>> AddPrankCallCost([FromBody]PrankCallCostModel prankCallCost)
        {
            var model = new ResponseModel();
            var result = await _dataContext.AddPrankCallCost(prankCallCost);

            if (!result.Status)
            {
                model.StatusCode = 404;
                model.Message = result.Message;
            }
            else
            {
                model.StatusCode = 200;
                var objPrankCostLst = await _dataContext.GetPrankCallCostListByPram(string.Empty).ToListAsync();
                if (objPrankCostLst != null)
                {
                    model.ResponseObject = objPrankCostLst;
                }
            }
            return model;
        }

        [HttpPost]
        [Route("UpdatePrankCallCost")]
        public async Task<ActionResult<ResponseModel>> UpdatePrankCallCost([FromBody]PrankCallCostModel prankCallCost)
        {
            var model = new ResponseModel();
            var result = await _dataContext.UpdatePrankCallCost(prankCallCost);

            if (!result.Status)
            {
                model.StatusCode = 404;
                model.Message = result.Message;
            }
            else
            {
                model.StatusCode = 200;
                var objPrankCostLst = await _dataContext.GetPrankCallCostListByPram(string.Empty).ToListAsync();
                if (objPrankCostLst != null)
                {
                    model.ResponseObject = objPrankCostLst;
                }
            }

            return model;
        }

    }
}