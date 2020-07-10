using Microsoft.Data.SqlClient;
using Prank.Model;
using System.Linq;
using System.Threading.Tasks;

namespace Prank.DAL
{
    public partial class DataContext
    {
        public IQueryable<ContactUsEmailTrackModel> GetContactUsEmailTrackByRequest(string EmailTo, string sortCol, string sortDir, int skip, int take)
        {
            SqlParameter[] parameter = { new SqlParameter("@EmailTo", EmailTo),
                new SqlParameter("@sortCol", sortCol),
                new SqlParameter("@sortDir", sortDir),
                new SqlParameter("@skip", skip),
                new SqlParameter("@take", take) };
            return FromSQLRAWWithParams<ContactUsEmailTrackModel>("[DBO].GetContactUsEmailTrackByRequest", parameter);
        }

        public async Task<DbStatusResult> AddContactUsRequest(ContactUsEmailTrackModel entity)
        {
            var cmd = new DbStatusCommand();

            await ExecuteSQLWithParams("[dbo].AddContactUsRequest",
                  GetJsonParam("@ContactUsEmailTrack", entity),
                  cmd.IdParam,
                  cmd.StatusParam,
                  cmd.MessageParam);

            return cmd.StatusResult;
        }

    }
}
