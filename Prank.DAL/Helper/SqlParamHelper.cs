using Microsoft.Data.SqlClient;
using Prank.DAL.Attributes;
using Prank.DAL.Attributes.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace Prank.DAL.Helper
{
    public static class SqlParamHelper
    {
        public static List<SqlParameter> GetParams<T>(T obj, bool isCamelCase = false)
        {
            var paramList = new List<SqlParameter>();
            var properties = typeof(T).GetProperties().Where(q => !q.GetCustomAttributes(typeof(DbParamNotMappedAttribute), true).Any()).ToList();

            foreach (var property in properties)
            {
                SqlDbType t = SqlDbType.VarChar;
                var propType = property.PropertyType;

                var propValue = property.GetValue(obj);
                var p = new SqlParameter(isCamelCase ? property.Name[0].ToString().ToLower() + property.Name.Substring(1) : property.Name, t);
                int size = p.Size;
                byte precision = p.Precision;
                byte scale = p.Scale;

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
                    size = -1;
                    var lengthAtt = property.GetCustomAttributes(typeof(StringLengthAttribute), true).FirstOrDefault() as StringLengthAttribute;
                    if (lengthAtt != null)
                    {
                        size = lengthAtt.MaximumLength;
                    }
                }
                else if (propType == typeof(DateTime) || propType == typeof(DateTime?))
                {
                    var dateAtt = property.GetCustomAttributes(typeof(DataTypeAttribute), true).FirstOrDefault();
                    if (dateAtt != null && ((DataTypeAttribute)dateAtt).DataType == DataType.Date)
                    {
                        t = SqlDbType.Date;
                    }
                    else
                    {
                        t = SqlDbType.DateTime;
                    }

                }
                else if (propType == typeof(bool) || propType == typeof(bool?))
                {
                    t = SqlDbType.Bit;
                }
                else if (propType == typeof(decimal) || propType == typeof(decimal?))
                {
                    var precScaleAtt = property.GetCustomAttributes(typeof(DecimalPrecisionAttribute), true).FirstOrDefault() as DecimalPrecisionAttribute;
                    if (precScaleAtt != null)
                    {
                        t = SqlDbType.Decimal;
                        precision = precScaleAtt.Precision;
                        scale = precScaleAtt.Scale;
                    }

                    t = SqlDbType.Decimal;
                }
                else if (propType == typeof(byte[]))
                {
                    t = SqlDbType.VarBinary;
                }


                p.Size = size;
                p.Precision = precision;
                p.Scale = scale;
                p.Value = (object)propValue ?? DBNull.Value;

                paramList.Add(p);
            }

            return paramList;
        }
    }
}
