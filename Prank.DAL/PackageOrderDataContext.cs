using Microsoft.Data.SqlClient;
using Prank.Model;
using System.Linq;
using System.Threading.Tasks;

namespace Prank.DAL
{
    public partial class DataContext
    {
        public IQueryable<PackageOrderModel> GetPackageOrderInfoById(int packageOrderId, int deviceId, int packageId, string orderStatus, string tokenEarnType)
        {
            try
            {
                orderStatus = !string.IsNullOrEmpty(orderStatus) ? orderStatus : string.Empty;
                tokenEarnType = !string.IsNullOrEmpty(tokenEarnType) ? tokenEarnType : string.Empty;

                SqlParameter[] parameter = { new SqlParameter("@PackageOrderId", packageOrderId),
                  new SqlParameter("@DeviceId", deviceId),
                  new SqlParameter("@PackageId", packageId),
                  new SqlParameter("@OrderStatus", orderStatus),
                 new SqlParameter("@TokenEarnType", tokenEarnType),
            };
                var sd = FromSQLRAWWithParams<PackageOrderModel>("[DBO].GetPackageOrderInfoById", parameter);
                return sd;
            }
            catch (System.Exception e)
            {
                throw;
            }
        }

        public async Task<DbStatusResult> AddPackageOrderInfo(PackageOrderModel entity)
        {
            var cmd = new DbStatusCommand();

            await ExecuteSQLWithParams("[dbo].AddPackageOrderInfo",
                  GetJsonParam("@PackageOrderInfo", entity),
                  cmd.IdParam,
                  cmd.StatusParam,
                  cmd.MessageParam);

            return cmd.StatusResult;
        }
        public IQueryable<PackageOrderModel> GetAvailablePackageOrderInfoByDeviceId(int deviceId)
        {
            SqlParameter[] parameter = {
                  new SqlParameter("@DeviceId", deviceId)
            };
            return FromSQLRAWWithParams<PackageOrderModel>("[DBO].GetAvailablePackageOrderInfoByDeviceId", parameter);
        }
    }
}
