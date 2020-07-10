using Microsoft.Data.SqlClient;
using Prank.Model;
using System.Linq;
using System.Threading.Tasks;


namespace Prank.DAL
{
    public partial class DataContext
    {
        public IQueryable<PrankCallFromPhoneNumberLstModel> GetPrankCallFromPhoneNumberByPram(string prankCallFromPhoneNumberGroup, int prankCallFromId, string search, int skip, int take, string sortCol, string sortDir)
        {
            SqlParameter[] parameter = { new SqlParameter("@PrankCallFromPhoneNumberGroup", prankCallFromPhoneNumberGroup),
                new SqlParameter("@PrankCallFromId", prankCallFromId),
                    new SqlParameter("@skip", skip),
                new SqlParameter("@take", take),
             new SqlParameter("@search", search),
             new SqlParameter("@sortCol", sortCol),
             new SqlParameter("@sortDir", sortDir)
            };
            return FromSQLRAWWithParams<PrankCallFromPhoneNumberLstModel>("[DBO].GetPrankCallFromPhoneNumberByPram", parameter);
        }

        public async Task<DbStatusResult> AddPrankCallFromPhoneNumber(PrankCallFromPhoneNumberModel entity)
        {
            var cmd = new DbStatusCommand();

            await ExecuteSQLWithParams("[dbo].AddPrankCallFromPhoneNumber",
                  GetJsonParam("@PrankCallFromPhoneNumber", entity),
                  cmd.IdParam,
                  cmd.StatusParam,
                  cmd.MessageParam);

            return cmd.StatusResult;
        }
        public async Task<DbStatusResult> UpdatePrankCallFromPhoneNumber(PrankCallFromPhoneNumberModel entity)
        {
            var cmd = new DbStatusCommand();

            await ExecuteSQLWithParams("[dbo].UpdatePrankCallFromPhoneNumber",
                  GetJsonParam("@PrankCallFromPhoneNumber", entity),
                  cmd.IdParam,
                  cmd.StatusParam,
                  cmd.MessageParam);

            return cmd.StatusResult;
        }
        public async Task<DbStatusResult> DeletePrankCallFromPhoneNumber(int prankCallFromId)
        {
            var cmd = new DbStatusCommand();

            await ExecuteSQLWithParams("[dbo].DeletePrankCallFromPhoneNumber",
                  new SqlParameter("@PrankCallFromId", prankCallFromId),
                  cmd.IdParam,
                  cmd.StatusParam,
                  cmd.MessageParam);

            return cmd.StatusResult;
        }

        public async Task<DbStatusResult> UpdateIsDefaultStatusFromPhoneNumber(int PrankCallFromId, bool IsDefault)
        {
            var cmd = new DbStatusCommand();

            await ExecuteSQLWithParams("[dbo].UpdateIsDefaultStatusFromPhoneNumber",
                  new SqlParameter("@PrankCallFromId", PrankCallFromId),
                  new SqlParameter("@IsDefault", IsDefault),
                  cmd.IdParam,
                  cmd.StatusParam,
                  cmd.MessageParam);

            return cmd.StatusResult;
        }

    }
}
