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
    public class PageContentController : BaseApiController
    {
        public PageContentController(DataContext dataContext, IConfiguration config, FileSettings fileSettings, IWebHostEnvironment env) : base(dataContext, config, fileSettings, env)
        {

        }

        [HttpGet]
        [Route("GetPageContentListByRequest")]
        public async Task<ActionResult<ResponseModel>> GetPageContentListByRequest(string pageContentKey, int pageContentId)
        {
            var model = new ResponseModel();
            var objPrankList = await _dataContext.GetPageContentByPram(pageContentKey, pageContentId).ToListAsync();

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
        [Route("UpdatePageContent")]
        public async Task<ActionResult<ResponseModel>> UpdatePageContent([FromBody] PageContentModel pageContent)
        {
            var model = new ResponseModel();
            var result = await _dataContext.UpdatePageContent(pageContent);

            if (!result.Status)
            {
                model.StatusCode = 404;
                model.Message = result.Message;
            }
            else
            {
                model.StatusCode = 200;
                var objPrankList = await _dataContext.GetPageContentByPram(string.Empty, 0).ToListAsync();
                if (objPrankList != null)
                {
                    model.ResponseObject = objPrankList;
                }
            }

            return model;
        }
    }
}
