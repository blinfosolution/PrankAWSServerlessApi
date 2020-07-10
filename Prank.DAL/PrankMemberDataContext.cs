using Microsoft.Data.SqlClient;
using Prank.Model;
using System.Linq;
using System.Threading.Tasks;

namespace Prank.DAL
{
    public partial class DataContext
    {

        public IQueryable<PrankMemberModel> GetPrankMemberListByRequest(string firstName)
        {
            SqlParameter[] parameter = { new SqlParameter("@FirstName", firstName) };
            return FromSQLRAWWithParams<PrankMemberModel>("[DBO].GetPrankMemberListByRequest", parameter);
        }

        public async Task<DbStatusResult> AddPrankMemberInfo(PrankMemberModel entity)
        {
            var cmd = new DbStatusCommand();

            await ExecuteSQLWithParams("[dbo].AddPrankMember",
                  GetJsonParam("@MemberInfo", entity),
                  cmd.IdParam,
                  cmd.StatusParam,
                  cmd.MessageParam);

            return cmd.StatusResult;
        }

        public IQueryable<PrankMemberModel> MemberLoginRequest(string useName,string password )
        {
            SqlParameter[] parameter = { new SqlParameter("@UserName", useName), new SqlParameter("@Password", password), };
            return FromSQLRAWWithParams<PrankMemberModel>("[DBO].MemberLoginRequest", parameter);
        }
    }
}
