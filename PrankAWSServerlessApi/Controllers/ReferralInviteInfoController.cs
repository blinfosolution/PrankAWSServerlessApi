using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Prank.DAL;
using PrankAWSServerlessApi.ApiSettings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Prank.Model;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;

namespace PrankAWSServerlessApi.Controllers
{
    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReferralInviteInfoController : BaseApiController
    {
        public ReferralInviteInfoController(DataContext dataContext, IConfiguration config, FileSettings fileSettings, IWebHostEnvironment env) : base(dataContext, config, fileSettings, env)
        {
        }
        [HttpGet]
        [Route("GetAllReferralInfoListByRequest")]
        public async Task<ActionResult<ResponseModel>> GetAllReferralInviteInfoLstByRequest(int fromDeviceId,string search, string sortCol, string sortDir, int skip, int take)
        {


            sortCol = string.IsNullOrEmpty(sortCol) ? "ReferralInviteId" : sortCol;
            sortDir = string.IsNullOrEmpty(sortDir) ? "desc" : sortDir;
            search = string.IsNullOrEmpty(search) ? "" : search;
            var model = new ResponseModel();

            var objRefferalInfokLst = new List<ReferralInviteListModel>();

            try
            {
                objRefferalInfokLst = await _dataContext.GetAllReferralInviteInfoLstByRequest(fromDeviceId,search, sortCol, sortDir, skip, take).ToListAsync();
            }
            catch(Exception ex)
            {
                string str = ex.ToString();
            }
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
        [Route("AddReferralInvitee")]
        public async Task<ActionResult<ResponseModel>> AddReferralInvitee([FromBody] ReferralInviteInfoModel referralInviteInfoModel)
        {
            var model = new ReferralInfoInviteResponseModel();
            var result = await _dataContext.AddReferralInviteInfo(referralInviteInfoModel);

            if (!result.Status)
            {
                model.StatusCode = 404;
                model.Message = result.Message;
            }
            else
            {
                model.StatusCode = 200;
                model.Id = result.Id;
            }

            return model;
        }

        [HttpPost]
        [Route("ReferralCallback")]
        public async Task<ActionResult<ResponseModel>> ReferralCallback(int deviceId,int referralInviteId,string referralLinkKey)
        {
            var model = new ReferralInfoInviteResponseModel();
            var result = await _dataContext.UpdateReferralInviteInfo(deviceId,referralInviteId,referralLinkKey);

            if (!result.Status)
            {
                model.StatusCode = 404;
                model.Message = result.Message;
            }
            else
            {
                model.StatusCode = 200;
                model.Id = result.Id;
              
            }

            return model;
        }

    }

}
