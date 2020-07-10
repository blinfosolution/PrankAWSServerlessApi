using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Prank.DAL;
using Prank.Model;
using PrankAWSServerlessApi.ApiSettings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace PrankAWSServerlessApi.Controllers
{
 [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PackageOrderController : BaseApiController
    {
        public PackageOrderController(DataContext dataContext, IConfiguration config, FileSettings fileSettings, IWebHostEnvironment env) : base(dataContext, config, fileSettings, env)
        {

        }
        [HttpGet]
        [Route("GetPackageOrderInfoById")]
        public async Task<ActionResult<ResponseModel>> GetPackageOrderInfoById(int packageOrderId, int deviceId, int packageId,string orderStatus,string tokenEarnType)
        {
            var model = new ResponseModel();
            var objPackageOrderLst = await _dataContext.GetPackageOrderInfoById(packageOrderId, deviceId, packageId, orderStatus, tokenEarnType).ToListAsync();

            if (objPackageOrderLst != null)
            {
                model.ResponseObject = objPackageOrderLst;
                model.StatusCode = 200;
            }
            else
            {
                model.StatusCode = 404;
            }

            return model;
        }

        [HttpPost]
        [Route("AddPackageOrder")]
        public async Task<ActionResult<ResponseModel>> AddPackageOrder([FromBody]PackageOrderModel packageOrder)
        {
            var model = new ResponseModel();
            var result = await _dataContext.AddPackageOrderInfo(packageOrder);

            if (!result.Status)
            {
                model.StatusCode = 404;
                model.Message = result.Message;
            }
            else
            {
                model.StatusCode = 200;
                var objDeviceList = await _dataContext.GetPackageOrderInfoById(0,0, 0, string.Empty, string.Empty).ToListAsync();
                if (objDeviceList != null)
                {
                    model.ResponseObject = objDeviceList;
                }
            }
            return model;
        }


        [HttpGet]
        [Route("GetAvailablePackageOrderInfoByDeviceId")]
        public async Task<ActionResult<ResponseModel>> GetAvailablePackageOrderInfoByDeviceId(int deviceId)
        {
            var model = new ResponseModel();
            var objDeviceList = await _dataContext.GetAvailablePackageOrderInfoByDeviceId( deviceId).ToListAsync();

            if (objDeviceList != null)
            {
                int TotalCreditPoint =objDeviceList.Count > 0 ? objDeviceList.Sum(s => s.TokenCreditPoint):0;
                model.ResponseObject = TotalCreditPoint;
                model.StatusCode = 200;
            }
            else
            {
                model.StatusCode = 404;
            }

            return model;
        }


    }
}