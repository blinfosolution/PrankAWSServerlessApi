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

        public IQueryable<TrackingInfoListModel> GetAllTrackingInfoListByRequest( string search, string sortCol, string sortDir, int skip, int take)
        {
            try
            {
                SqlParameter[] parameter = { new SqlParameter("@search", search),
                    new SqlParameter("@sortCol", sortCol),
                    new SqlParameter("@sortDir", sortDir),
                    new SqlParameter("@skip", skip),
                    new SqlParameter("@take", take) 
                };
                var s = FromSQLRAWWithParams<TrackingInfoListModel>("[DBO].GetAllTrackingInfoListByRequest", parameter);
                return s;
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public async Task<DbStatusResult> AddTrackingInfo(TrackingInfoModel entity)
        {
            var cmd = new DbStatusCommand();

            await ExecuteSQLWithParams("[dbo].AddTrackingInfo",
                  GetJsonParam("@TrackingInfo", entity),
                  cmd.IdParam,
                  cmd.StatusParam,
                  cmd.MessageParam);

            return cmd.StatusResult;
        }


    }
}
