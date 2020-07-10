using Microsoft.Data.SqlClient;
using Prank.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prank.DAL
{
    public partial class DataContext
    {
        public IQueryable<ReferralInviteListModel> GetAllReferralInviteInfoLstByRequest(int fromDeviceId,string search, string sortCol, string sortDir, int skip, int take)
        {
           
            SqlParameter[] parameter = { new SqlParameter("@DeviceId", fromDeviceId),
                 new SqlParameter("@sortCol", sortCol),
                 new SqlParameter("@sortDir", sortDir),
                new SqlParameter("@skip", skip),
                new SqlParameter("@take", take),
                  new SqlParameter("@search", search)

            };
            var res= FromSQLRAWWithParams<ReferralInviteListModel>("[DBO].GetAllReferralInviteInfoLstByRequest", parameter).ToList();
            return FromSQLRAWWithParams<ReferralInviteListModel>("[DBO].GetAllReferralInviteInfoLstByRequest", parameter);

        }

        public async Task<DbStatusResult> AddReferralInviteInfo(ReferralInviteInfoModel entity)
        {
            var cmd = new DbStatusCommand();
            await ExecuteSQLWithParams("[dbo].AddReferralInvitee",
                  GetJsonParam("@ReferralInvitee", entity),
                  cmd.IdParam,
                  cmd.StatusParam,
                  cmd.MessageParam);

            return cmd.StatusResult;
        }

        public async Task<DbStatusResult> UpdateReferralInviteInfo(int deviceId,int referralInviteId, string referralLinkKey)
        {
            var cmd = new DbStatusCommand();
            await ExecuteSQLWithParams("[dbo].UpdateReferralInviteeCallback",
                  new SqlParameter("@DeviceId", deviceId),
                  new SqlParameter("@ReferralInviteId", referralInviteId),
                  new SqlParameter("@ReferralLinkKey", referralLinkKey),
                  cmd.IdParam,
                  cmd.StatusParam,
                  cmd.MessageParam);

            return cmd.StatusResult;
        }
    }
}
