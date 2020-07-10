using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace Prank.DAL.Helper
{
    public static class DbHelpers
    {
        public static SqlDbType MapSqlParamType(Type propType)
        {
            SqlDbType t = SqlDbType.VarChar;
            if (propType == typeof(int) || propType == typeof(int?))
            {
                t = SqlDbType.Int;
            }
            else if (propType == typeof(short) || propType == typeof(short?))
            {
                t = SqlDbType.SmallInt;
            }
            else if (propType == typeof(string))
            {
                t = SqlDbType.VarChar;
            }
            else if (propType == typeof(DateTime) || propType == typeof(DateTime?))
            {
                t = SqlDbType.DateTime;
            }
            else if (propType == typeof(bool) || propType == typeof(bool?))
            {
                t = SqlDbType.Bit;
            }
            else if (propType == typeof(decimal) || propType == typeof(decimal?))
            {
                t = SqlDbType.Decimal;
            }
            else if (propType == typeof(byte[]))
            {
                t = SqlDbType.VarBinary;
            }
            return t;
        }

        public static string GetQuery(string sql, params SqlParameter[] parameters)
        {
            string query = sql;
            int i = 0;
            foreach (var p in parameters)
            {
                if (i > 0)
                {
                    query += ",";
                }
                query += $" {p.ParameterName}";
                if (p.Direction == ParameterDirection.Output)
                {
                    query += " output";
                }

                i++;
            }
            return query;
        }
    }
}
