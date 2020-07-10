using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Prank.DAL.Helper;
using Prank.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Prank.DAL
{
    public partial class DataContext : DbContext
    {

        private readonly string _connectionString;
        IMapper mapper;
        public DataContext()
        {

        }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PrankInfoModel>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<PrankInfoListModel>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<PackagesModel>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<PageContentModel>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<PrankCallFromPhoneNumberModel>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<PrankCallFromPhoneNumberLstModel>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<DeviceInfoModel>(entity =>
            {
                entity.HasNoKey();
            }); modelBuilder.Entity<DeviceInfoListModel>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<PrankCallTrackingModel>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<PackageOrderModel>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<PrankCallHistoryModel>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<ReferralInfoModel>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<ReferralInfoListModel>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<PrankCallCostModel>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<PrankMemberModel>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<ContactUsEmailTrackModel>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<ReferralInviteListModel>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<TrackingInfoModel>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<TrackingInfoListModel>(entity =>
            {
                entity.HasNoKey();
            });

        }

        #region common params

        private class DbStatusCommand
        {
            public SqlParameter IdParam { get; private set; }
            public SqlParameter LongIdParam { get; private set; }
            public SqlParameter StatusParam { get; private set; }
            public SqlParameter MessageParam { get; private set; }
            public SqlParameter JsonResultParam { get; private set; }
            public SqlParameter GuidParam { get; private set; }

            public int Id => IdParam.Value is DBNull ? (int)0 : (int)IdParam.Value;
            public long LongId => LongIdParam.Value is DBNull ? (long)0 : (long)LongIdParam.Value;
            public bool Status => StatusParam.Value is DBNull ? false : (bool)StatusParam.Value;
            public string Message => MessageParam.Value is DBNull ? null : (string)MessageParam.Value;
            public string JsonResult => JsonResultParam.Value is DBNull ? null : (string)JsonResultParam.Value;
            public string/*Guid*/ Guid => GuidParam.Value is DBNull ? null : (string)GuidParam.Value; //Guid.Empty : (Guid)GuidParam.Value;


            public DbStatusResult StatusResult => new DbStatusResult { Id = Id, Message = Message, Status = Status, ResultData = JsonResult, Guid = Guid, LongId = LongId };

            public DbStatusCommand()
            {
                IdParam = DataContext.GetIdParam();
                LongIdParam = DataContext.GetLongIdParam();
                GuidParam = DataContext.GetGuidParam();
                StatusParam = DataContext.GetStatusParam();
                MessageParam = DataContext.GetMessageParam();
                JsonResultParam = new SqlParameter
                {
                    ParameterName = "@result",
                    Direction = ParameterDirection.Output,
                    SqlDbType = SqlDbType.NVarChar,
                    Size = -1
                };
            }
        }

        public static SqlParameter GetIdParam(ParameterDirection dir = ParameterDirection.Output, int id = 0)
        {
            return new SqlParameter
            {
                ParameterName = "@id",
                Value = id,
                Direction = dir,
                SqlDbType = SqlDbType.Int
            };
        }
        public static SqlParameter GetLongIdParam(ParameterDirection dir = ParameterDirection.Output, long id = 0)
        {
            return new SqlParameter
            {
                ParameterName = "@id",
                Value = id,
                Direction = dir,
                SqlDbType = SqlDbType.BigInt
            };
        }
        public static SqlParameter GetCodeParam(string code, int length = 2)
        {
            return new SqlParameter
            {
                ParameterName = "@id",
                Value = code,
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.VarChar,
                Size = length
            };
        }
        public static SqlParameter GetStatusParam()
        {
            return new SqlParameter
            {
                ParameterName = "@status",
                Value = 0,
                Direction = ParameterDirection.Output,
                SqlDbType = SqlDbType.Bit
            };
        }
        public static SqlParameter GetMessageParam()
        {
            return new SqlParameter
            {
                ParameterName = "@message",
                Value = "",
                Direction = ParameterDirection.Output,
                SqlDbType = SqlDbType.VarChar,
                Size = -1
            };
        }
        private SqlParameter GetUserNameParam(string userName)
        {
            return new SqlParameter
            {
                ParameterName = "@userName",
                Value = userName,
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.VarChar,
                Size = 50
            };
        }
        private SqlParameter GetUserIdParam(int id)
        {
            return new SqlParameter
            {
                ParameterName = "@userId",
                Value = id,
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.SmallInt
            };
        }
        public static SqlParameter GetGuidParam()
        {
            return new SqlParameter
            {
                Value = "",
                ParameterName = "@guid",
                Direction = ParameterDirection.Output,
                SqlDbType = SqlDbType.VarChar,//SqlDbType.UniqueIdentifier
                Size = 36
            };
        }
        public static SqlParameter GetJsonParam(string name, object value)
        {
            return new SqlParameter
            {
                ParameterName = name,
                Value = value != null ?  (object)Newtonsoft.Json.JsonConvert.SerializeObject(value)  : DBNull.Value,
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.NVarChar
            };
        }
        #endregion

        #region helpers
        private void RunInTransaction(Action action)
        {
            using (var transaction = this.Database.BeginTransaction())
            {
                try
                {
                    action();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
        }
        private T RunInTransaction<T>(Func<T> action)
        {
            using (var transaction = this.Database.BeginTransaction())
            {
                try
                {
                    var result = action();
                    transaction.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    //transaction.Rollback();
                    throw ex;
                }
            }
        }
        private async Task<T> RunInTransactionAsync<T>(Func<Task<T>> action)
        {
            using (var transaction = this.Database.BeginTransaction())
            {
                try
                {
                    var result = await action();
                    transaction.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        private async Task RunInTransactionAsync(Func<Task> action)
        {
            using (var transaction = this.Database.BeginTransaction())
            {
                try
                {
                    await action();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        [Obsolete]
        private IQueryable<T> FromSQLWithParams<T>(string sql, params SqlParameter[] parameters) where T : class
        {
            string query = DbHelpers.GetQuery(sql, parameters);
            return this.Query<T>().FromSql(query, parameters);
        }

        private IQueryable<T> FromSQLRAWWithParams<T>(string sql, params SqlParameter[] parameters) where T : class
        {
            string query = DbHelpers.GetQuery(sql, parameters);
            return this.Query<T>().FromSqlRaw(query, parameters);
        }

        [Obsolete]
        private IQueryable<T> FromSQLWithOutParams<T>(string sql) where T : class
        {
            string query = DbHelpers.GetQuery(sql);
            return this.Query<T>().FromSql(query);
        }

        private IQueryable<T> FromSQLRAWWithOutParams<T>(string sql) where T : class
        {
            string query = DbHelpers.GetQuery(sql);
            return this.Query<T>().FromSqlRaw(query);
        }
        private Task<int> ExecuteSQLWithParams(string sql, params SqlParameter[] parameters)
        {
            string query = DbHelpers.GetQuery(sql, parameters);
            return this.Database.ExecuteSqlRawAsync(query, parameters);
            //return this.Database.ExecuteSqlCommandAsync(query, parameters);
        }

        private Task<int> ExecuteSQL(string sql)
        {
            return this.Database.ExecuteSqlRawAsync(sql);
            //return this.Database.ExecuteSqlCommandAsync(sql);
        }

        #endregion     

        public DataContext(string connectionString)
        {
            _connectionString = connectionString;
            this.Database.SetCommandTimeout(TimeSpan.FromMinutes(2));
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
            //base.OnConfiguring(optionsBuilder);
        }

        #region generic ef helpers
        protected virtual async Task<bool> AddEntity<TEntity, TModel>(TModel model) where TEntity : class
        {
            //TEntity entity = Mapper.Map<TEntity>(model);
            TEntity entity = mapper.Map<TEntity>(model);
            this.Add(entity);
            var entry = this.Entry(entity);
            var primaryKey = entry.Metadata.FindPrimaryKey();
            var key = primaryKey.Properties.Select(x => new { Name = x.Name, Value = x.PropertyInfo.GetValue(entity) }).FirstOrDefault();
            var prop = typeof(TModel).GetProperty(key.Name);
            var status = await this.SaveChangesAsync() > 0;
            prop.SetValue(model, key.Value);
            return status;
        }

        protected virtual async Task<bool> UpdateEntity<TEntity, TModel>(TModel model, object id, List<string> toSkipProperties = null, List<string> toUpdateProperties = null) where TEntity : class
        {
            var entity = await this.FindAsync(typeof(TEntity), id);
            if (entity == null)
            {
                throw new Exception("Object not found");
            }
            if (toUpdateProperties != null)
            {
                foreach (var prop in toUpdateProperties)
                {
                    this.Entry(entity).Property(prop).CurrentValue = typeof(TModel).GetProperty("prop").GetValue(model);
                    this.Entry(entity).Property(prop).IsModified = true;
                }
            }
            else
            {
                this.Entry(entity).CurrentValues.SetValues(model);
                this.Entry(entity).State = EntityState.Modified;
                if (toSkipProperties != null)
                {
                    foreach (var s in toSkipProperties)
                    {
                        this.Entry(entity).Property(s).IsModified = false;
                    }
                }
            }
            return await this.SaveChangesAsync() > 0;
        }

        protected virtual async Task<bool> DeleteEntity<TEntity>(object id) where TEntity : class
        {
            var entity = await this.FindAsync(typeof(TEntity), id);
            if (entity == null)
            {
                throw new Exception("Object not found");
            }
            this.Entry(entity).State = EntityState.Deleted;
            return await this.SaveChangesAsync() > 0;

        }
        protected virtual async Task<TModel> GetEntity<TEntity, TModel>(object id) where TEntity : class
        {
            var entity = await this.FindAsync(typeof(TEntity), id);
            if (entity == null)
            {
                throw new Exception("Object not found");
            }
            return mapper.Map<TModel>(entity);
            //return Mapper.Map<TModel>(entity);
        }
        #endregion
    }
}
