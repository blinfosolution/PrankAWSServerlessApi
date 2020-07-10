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
        public IQueryable<PrankCallFromPhoneNumberModel> PrankCallFromPhoneNumber(string toPhoneNumberCountryCodePrefix)
        {
            SqlParameter[] parameter = { new SqlParameter("@toPhoneNumberCountryCodePrefix", toPhoneNumberCountryCodePrefix) };
            return FromSQLRAWWithParams<PrankCallFromPhoneNumberModel>("[DBO].[GetCodePrankCallFromPhoneNumber ]", parameter);
        }

        public async Task<DbStatusResult> AddPrankCallTracking(PrankCallTrackingModel entity)
        {
            var cmd = new DbStatusCommand();

            await ExecuteSQLWithParams("[dbo].AddPrankCallTracking",
                  GetJsonParam("@PrankCallTrackingInfo", entity),
                  cmd.IdParam,
                  cmd.StatusParam,
                  cmd.MessageParam);

            return cmd.StatusResult;
        }

        public IQueryable<PrankCallHistoryModel> GetPrankCallHistoryList( int deviceId,int trackingId ,int skip, int take)
        {
            SqlParameter[] parameter = { new SqlParameter("@DeviceId", deviceId),
                new SqlParameter("@TrackingId", trackingId),
            new SqlParameter("@skip", skip),
            new SqlParameter("@take", take)

            };
            return FromSQLRAWWithParams<PrankCallHistoryModel>("[DBO].GetPrankCallHistoryList", parameter);
        }

        public async Task<DbStatusResult> DeleteCallHistory(int trackingId)
        {
            var cmd = new DbStatusCommand();

            await ExecuteSQLWithParams("[dbo].DeleteCallHistory",
                  new SqlParameter("@TrackingId", trackingId),
                  cmd.IdParam,
                  cmd.StatusParam,
                  cmd.MessageParam);

            return cmd.StatusResult;
        }
    }
}
