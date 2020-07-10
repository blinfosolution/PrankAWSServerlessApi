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

        public IQueryable<PrankCallCostModel> GetPrankCallCostListByPram(string countryPrefix)
        {
            SqlParameter[] parameter = { new SqlParameter("@CountryPrefix", countryPrefix) };
            return FromSQLRAWWithParams<PrankCallCostModel>("[DBO].GetPrankCallCostListByPram", parameter);
        }
          public async Task<DbStatusResult> AddPrankCallCost(PrankCallCostModel entity)
        {
            var cmd = new DbStatusCommand();
            await ExecuteSQLWithParams("[dbo].AddPrankCallCost",
                  GetJsonParam("@PrankCallCost", entity),
                  cmd.IdParam,
                  cmd.StatusParam,
                  cmd.MessageParam);

            return cmd.StatusResult;
        }
        public async Task<DbStatusResult> UpdatePrankCallCost(PrankCallCostModel entity)
        {
            var cmd = new DbStatusCommand();

            await ExecuteSQLWithParams("[dbo].UpdatePrankCallCost",
                  GetJsonParam("@PrankCallCost", entity),
                  cmd.IdParam,
                  cmd.StatusParam,
                  cmd.MessageParam);

            return cmd.StatusResult;
        }
    }
}
