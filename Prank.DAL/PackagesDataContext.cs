using Microsoft.Data.SqlClient;
using Prank.Model;
using System.Linq;
using System.Threading.Tasks;

namespace Prank.DAL
{
    public partial class DataContext
    {
        public IQueryable<PackagesModel> GetPackagesByPram( int prankId, string sortCol, string sortDir, int skip, int take)
        {
            SqlParameter[] parameter = {  new SqlParameter("@PackageId", prankId),
                new SqlParameter("@sortCol", sortCol),
                new SqlParameter("@sortDir", sortDir),
                new SqlParameter("@skip", skip),
                new SqlParameter("@take", take) 
            };
            return FromSQLRAWWithParams<PackagesModel>("[DBO].GetPackagesByPram", parameter);
        }

        public async Task<DbStatusResult> AddPackages(PackagesModel entity)
        {
            var cmd = new DbStatusCommand();

            await ExecuteSQLWithParams("[dbo].AddPackages",
                  GetJsonParam("@Packages", entity),
                  cmd.IdParam,
                  cmd.StatusParam,
                  cmd.MessageParam);

            return cmd.StatusResult;
        }
        public async Task<DbStatusResult> UpdatePackages(PackagesModel entity)
        {
            var cmd = new DbStatusCommand();

            await ExecuteSQLWithParams("[dbo].UpdatePackages",
                  GetJsonParam("@Packages", entity),
                  cmd.IdParam,
                  cmd.StatusParam,
                  cmd.MessageParam);

            return cmd.StatusResult;
        }
        public async Task<DbStatusResult> DeletePackages(int PrankId)
        {
            var cmd = new DbStatusCommand();

            await ExecuteSQLWithParams("[dbo].DeletePackages",
                  new SqlParameter("@PackageId", PrankId),
                  cmd.IdParam,
                  cmd.StatusParam,
                  cmd.MessageParam);

            return cmd.StatusResult;
        }
    }
}
