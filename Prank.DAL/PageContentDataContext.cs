using Microsoft.Data.SqlClient;
using Prank.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Prank.DAL
{
    public partial class DataContext
    {
        public IQueryable<PageContentModel> GetPageContentByPram(string pageContentGroup, int pageContentId)
        {
            SqlParameter[] parameter = { new SqlParameter("@PageContentGroup", pageContentGroup), new SqlParameter("@PageContentId", pageContentId) };
            return FromSQLRAWWithParams<PageContentModel>("[DBO].GetPageContentByPram", parameter);
        }

     
        public async Task<DbStatusResult> UpdatePageContent(PageContentModel entity)
        {
            var cmd = new DbStatusCommand();

            await ExecuteSQLWithParams("[dbo].UpdatePageContent",
                  GetJsonParam("@PageContent", entity),
                  cmd.IdParam,
                  cmd.StatusParam,
                  cmd.MessageParam);

            return cmd.StatusResult;
        }
        //public async Task<DbStatusResult> DeletePageContentInfo(int PageContentId)
        //{
        //    var cmd = new DbStatusCommand();

        //    await ExecuteSQLWithParams("[dbo].DeletePageContentInfo",
        //          new SqlParameter("@PageContentId", PageContentId),
        //          cmd.IdParam,
        //          cmd.StatusParam,
        //          cmd.MessageParam);

        //    return cmd.StatusResult;
        //}
    }
}
