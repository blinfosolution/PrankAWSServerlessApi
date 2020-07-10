using Microsoft.Data.SqlClient;
using Prank.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prank.DAL
{
  public partial  class DataContext 
    {
        public IQueryable<ReferralInfoModel> GetReferralInfoListByRequest()
        {
            return FromSQLRAWWithOutParams<ReferralInfoModel>("[DBO].GetReferralInfoListByRequest");
        }

        public IQueryable<ReferralInfoListModel> GetAllReferralInfoListByRequest(int referralId,int skip,int take)
        {
            SqlParameter[] parameter = { new SqlParameter("@ReferralId", referralId),
                new SqlParameter("@skip", skip),
                new SqlParameter("@take", take) };
            return FromSQLRAWWithParams<ReferralInfoListModel>("[DBO].GetAllReferralInfoListByRequest", parameter);
        }
        public async Task<DbStatusResult> AddReferralInfo(ReferralInfoModel entity)
        {
            var cmd = new DbStatusCommand();

            await ExecuteSQLWithParams("[dbo].AddReferralInfo",
                  GetJsonParam("@ReferalInfo", entity),
                  cmd.IdParam,
                  cmd.StatusParam,
                  cmd.MessageParam);

            return cmd.StatusResult;
        }
        public async Task<DbStatusResult> UpdatePrankReferralInfo(ReferralInfoModel entity)
        {
            try
            {
                var cmd = new DbStatusCommand();

                await ExecuteSQLWithParams("[dbo].UpdatePrankReferralInfo",
                      GetJsonParam("@ReferralInfo", entity),
                      cmd.IdParam,
                      cmd.StatusParam,
                      cmd.MessageParam);

                return cmd.StatusResult;
            }
            catch (Exception er)
            {

                throw;
            }
        }
        public async Task<DbStatusResult> DeleteReferralInfo(int ReferralId)
        {
            var cmd = new DbStatusCommand();
            await ExecuteSQLWithParams("[dbo].DeleteReferralInfo",
                  new SqlParameter("@ReferralId", ReferralId),
                  cmd.IdParam,
                  cmd.StatusParam,
                  cmd.MessageParam);

            return cmd.StatusResult;
        }

    }
}
