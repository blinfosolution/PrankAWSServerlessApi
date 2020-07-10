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
    [Route("api/[controller]")]
    [ApiController]
    public class PrankMemberController : BaseApiController
    {
        public PrankMemberController(DataContext dataContext, IConfiguration config, FileSettings fileSettings, IWebHostEnvironment env) : base(dataContext, config, fileSettings, env)
        {
        }

        [HttpGet]
        [Route("GetPrankMemberListByRequest")]
        public async Task<ActionResult<ResponseModel>> GetPrankMemberListByRequest(string firstName)
        {
            var model = new ResponseModel();
            var objPrankCostLst = await _dataContext.GetPrankMemberListByRequest(firstName).ToListAsync();

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
        [Route("AddPrankMember")]
        public async Task<ActionResult<ResponseModel>> AddPrankMember([FromBody]PrankMemberModel memberInfo)
        {
            var model = new ResponseModel();
            var result = await _dataContext.AddPrankMemberInfo(memberInfo);

            if (!result.Status)
            {
                model.StatusCode = 404;
                model.Message = result.Message;
            }
            else
            {
                model.StatusCode = 200;
                var objPrankMemberLst = await _dataContext.GetPrankMemberListByRequest(string.Empty).ToListAsync();
                if (objPrankMemberLst != null)
                {
                    model.ResponseObject = objPrankMemberLst;
                }
            }
            return model;
        }

    }
}