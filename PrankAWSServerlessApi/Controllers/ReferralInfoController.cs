using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Prank.DAL;
using Prank.Model;
using PrankAWSServerlessApi.ApiSettings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Text;

namespace PrankAWSServerlessApi.Controllers
{
   [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReferralInfoController : BaseApiController
    {
        public ReferralInfoController(DataContext dataContext, IConfiguration config, FileSettings fileSettings, IWebHostEnvironment env) : base(dataContext, config, fileSettings, env)
        {
        }




        [HttpGet]
        [Route("GetReferralCodeByRequest")]
        public async Task<ActionResult<ResponseModel>> GetReferralCodeByRequest()
        {
            var model = new ResponseModel();
            var objRefferalInfokLst = await _dataContext.GetReferralInfoListByRequest().ToListAsync();

            if (objRefferalInfokLst != null && objRefferalInfokLst.Any())
            {
                int length = 7;
                StringBuilder str_build = new StringBuilder();
                Random random = new Random();
                char letter;
                for (int i = 0; i < length; i++)
                {
                    double flt = random.NextDouble();
                    int shift = Convert.ToInt32(Math.Floor(25 * flt));
                    letter = Convert.ToChar(shift + 65);
                    str_build.Append(letter);
                }

                var objRefrralInfo = objRefferalInfokLst.FirstOrDefault();
                objRefrralInfo.ReferralCode = str_build.ToString();

                model.ResponseObject = objRefrralInfo;
                model.StatusCode = 200;
            }
            else
            {
                model.StatusCode = 404;
            }

            return model;
        }


        [HttpGet]
        [Route("GetAllReferralInfoListByRequest")]
        public async Task<ActionResult<ResponseModel>> GetAllReferralInfoListByRequest(int referralId, int skip, int take)
        {
            var model = new ResponseModel();
            var objRefferalInfokLst = await _dataContext.GetAllReferralInfoListByRequest(referralId, skip, take).ToListAsync();

            if (objRefferalInfokLst != null)
            {
                model.ResponseObject = objRefferalInfokLst;
                model.StatusCode = 200;
            }
            else
            {
                model.StatusCode = 404;
            }

            return model;
        }


        [HttpPost]
        [Route("AddReferralInfo")]
        public async Task<ActionResult<ResponseModel>> AddReferralInfo([FromBody] ReferralInfoModel referalInfo)
        {
            var model = new ResponseModel();
            var result = await _dataContext.AddReferralInfo(referalInfo);

            if (!result.Status)
            {
                model.StatusCode = 404;
                model.Message = result.Message;
            }
            else
            {
                model.StatusCode = 200;
                var objPrankList = await _dataContext.GetAllReferralInfoListByRequest(0, 0, 10000).ToListAsync();
                if (objPrankList != null)
                {
                    model.ResponseObject = objPrankList;
                }
            }
            return model;
        }

        [HttpPost]
        [Route("UpdatePrankReferalInfo")]
        public async Task<ActionResult<ResponseModel>> UpdatePrankReferalInfo([FromBody] ReferralInfoModel referalInfo)
        {
            var model = new ResponseModel();
            var result = await _dataContext.UpdatePrankReferralInfo(referalInfo);

            if (!result.Status)
            {
                model.StatusCode = 404;
                model.Message = result.Message;
            }
            else
            {
                model.StatusCode = 200;
                var objPrankList = await _dataContext.GetAllReferralInfoListByRequest(0, 0, 10000).ToListAsync();
                if (objPrankList != null)
                {
                    model.ResponseObject = objPrankList;
                }
            }

            return model;
        }

        [HttpDelete]
        [Route("DeletePrankReferalInfo/{ReferralId}")]
        public async Task<ActionResult<ResponseModel>> DeletePrankReferalInfo(int ReferralId)
        {
            var model = new ResponseModel();
            var result = await _dataContext.DeleteReferralInfo(ReferralId);

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