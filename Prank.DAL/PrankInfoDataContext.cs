using Microsoft.Data.SqlClient;
using Prank.Model;
using System.Linq;
using System.Threading.Tasks;

namespace Prank.DAL
{
    public partial class DataContext
    {
        public IQueryable<PrankInfoListModel> GetAllPranksListByPram(string prankGroup, string search, int skip, int take, string sortCol, string sortDir)
        {
            SqlParameter[] parameter = { new SqlParameter("@PrankGroup", prankGroup),
                new SqlParameter("@skip", skip),
                new SqlParameter("@take", take),
             new SqlParameter("@search", search),
             new SqlParameter("@sortCol", sortCol),
             new SqlParameter("@sortDir", sortDir)
            };
            return FromSQLRAWWithParams<PrankInfoListModel>("[DBO].GetPranksInfoList", parameter);
        }

        public IQueryable<PrankInfoListModel> GetPranksListByPram(string prankGroup, int skip, int take)
        {
            SqlParameter[] parameter = { new SqlParameter("@PrankGroup", prankGroup),
                new SqlParameter("@skip", skip),
                new SqlParameter("@take", take) };
            return FromSQLRAWWithParams<PrankInfoListModel>("[DBO].GetPranksByPram", parameter);
        }
        public IQueryable<PrankInfoListModel> GetPrankInfoById(int prankId)
        {
            SqlParameter[] parameter = { new SqlParameter("@PrankId", prankId) };
            return FromSQLRAWWithParams<PrankInfoListModel>("[DBO].GetPrankInfoById", parameter);
        }



        public async Task<DbStatusResult> AddPrankInfo(PrankInfoModel entity)
        {
            var cmd = new DbStatusCommand();
            await ExecuteSQLWithParams("[dbo].AddPrankInfo",
                  GetJsonParam("@PrankInfo", entity),
                  cmd.IdParam,
                  cmd.StatusParam,
                  cmd.MessageParam);

            return cmd.StatusResult;
        }
        public async Task<DbStatusResult> UpdatePrankInfo(PrankInfoModel entity)
        {
            var cmd = new DbStatusCommand();

            await ExecuteSQLWithParams("[dbo].UpdatePrankInfo",
                  GetJsonParam("@PrankInfo", entity),
                  cmd.IdParam,
                  cmd.StatusParam,
                  cmd.MessageParam);

            return cmd.StatusResult;
        }
        public async Task<DbStatusResult> DeletePrankInfo(int PrankId)
        {
            var cmd = new DbStatusCommand();

            await ExecuteSQLWithParams("[dbo].DeletePrankInfo",
                  new SqlParameter("@PrankId", PrankId),
                  cmd.IdParam,
                  cmd.StatusParam,
                  cmd.MessageParam);

            return cmd.StatusResult;
        }
    }
}
