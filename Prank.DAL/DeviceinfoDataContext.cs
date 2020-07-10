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
        public async Task<DbStatusResult> ValidateDeviceInfoRequest(DeviceInfoModel entity)
        {
            var cmd = new DbStatusCommand();

            await ExecuteSQLWithParams("[dbo].ValidateDeviceInfoRequest",
                  GetJsonParam("@DeviceInfo", entity),
                  cmd.IdParam,
                  cmd.StatusParam,
                  cmd.MessageParam);

            return cmd.StatusResult;
        }
        public IQueryable<DeviceInfoListModel> GetDeviceInfoByKey(string deviceKey,string search, string sortCol, string sortDir, int skip, int take)
        {
            try
            {
                SqlParameter[] parameter = { new SqlParameter("@DeviceKey", deviceKey),
                new SqlParameter("@search", search),                
                new SqlParameter("@skip", skip),
            new SqlParameter("@take", take) , new SqlParameter("@sortCol", sortCol),
                    new SqlParameter("@sortDir", sortDir)};
                var s= FromSQLRAWWithParams<DeviceInfoListModel>("[DBO].GetDeviceInfoByKey", parameter);
                return s;
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public async Task<DbStatusResult> AddDeviceInfo(DeviceInfoModel entity)
        {
            var cmd = new DbStatusCommand();

            await ExecuteSQLWithParams("[dbo].AddDeviceInfo",
                  GetJsonParam("@DeviceInfo", entity),
                  cmd.IdParam,
                  cmd.StatusParam,
                  cmd.MessageParam);

            return cmd.StatusResult;
        }

        public async Task<DbStatusResult> UpdateStatusDeviceInfo(int deviceId,bool isActive)
        {
            var cmd = new DbStatusCommand();

            await ExecuteSQLWithParams("[dbo].UpdateStatusDeviceInfo",
                  new SqlParameter("@DeviceId", deviceId),
                  new SqlParameter("@IsActive", isActive),
                  cmd.IdParam,
                  cmd.StatusParam,
                  cmd.MessageParam);

            return cmd.StatusResult;
        }

        public async Task<DbStatusResult> UpdateDeviceInfo(DeviceInfoModel entity)
        {
            var cmd = new DbStatusCommand();

            await ExecuteSQLWithParams("[dbo].UpdateDeviceInfo",
                  GetJsonParam("@DeviceInfo", entity),
                  cmd.IdParam,
                  cmd.StatusParam,
                  cmd.MessageParam);

            return cmd.StatusResult;
        }
        public async Task<DbStatusResult> DeleteDeviceInfo(int deviceId)
        {
            var cmd = new DbStatusCommand();

            await ExecuteSQLWithParams("[dbo].DeleteDeviceInfo",
                  new SqlParameter("@DeviceId", deviceId),
                  cmd.IdParam,
                  cmd.StatusParam,
                  cmd.MessageParam);

            return cmd.StatusResult;
        }
    }
}
