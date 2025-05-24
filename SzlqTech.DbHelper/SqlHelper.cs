using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SzlqTech.DbHelper
{
    public sealed class SqlHelper
    {
        public const int Deleted = 1;

        public const int NotDeleted = 0;

        public static bool RetBool(int? result)
        {
            if (result.HasValue)
            {
                return result >= 1;
            }

            return false;
        }

        public static bool RetEqual(int? result1, int? result2)
        {
            if (result1.HasValue && result2.HasValue)
            {
                return result1 == result2;
            }

            return false;
        }

        public static bool RetBool(long? result)
        {
            if (result.HasValue)
            {
                return result >= 1;
            }

            return false;
        }

        public static long RetCount(long? result)
        {
            return result.GetValueOrDefault();
        }

        public static bool IsWithLogicDelete(Type t)
        {
            object[] customAttributes = t.GetCustomAttributes(typeof(LogicDeleteAttribute), inherit: false);
            if (customAttributes.Length != 0)
            {
                return (customAttributes[0] as LogicDeleteAttribute)?.LogicDelete ?? false;
            }

            return false;
        }

        private static (SqlSugarClient, DbType) GetDb()
        {
            string connectionString = (DbAndApiAuthConfig.Config.New ? DbAndApiAuthConfig.Config.DbConnectionString : ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString);
            DbType dbType = (DbAndApiAuthConfig.Config.New ? DbAndApiAuthConfig.Config.DbType : DbType.Sqlite);
            return (new SqlSugarClient(new ConnectionConfig
            {
                ConnectionString = connectionString,
                DbType = dbType,
                IsAutoCloseConnection = true
            }), dbType);
        }

        public static void BackupDatabase(string fullFileName)
        {
            string text = (DbAndApiAuthConfig.Config.New ? DbAndApiAuthConfig.Config.DbConnectionString : ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString);
            DbType dbType = (DbAndApiAuthConfig.Config.New ? DbAndApiAuthConfig.Config.DbType : DbType.Sqlite);
            SqlSugarClient sqlSugarClient = new SqlSugarClient(new ConnectionConfig
            {
                ConnectionString = text,
                DbType = dbType,
                IsAutoCloseConnection = true
            });
            if ((uint)dbType <= 1u)
            {
                sqlSugarClient.DbMaintenance.BackupDataBase(sqlSugarClient.Ado.Connection.Database, fullFileName);
            }
            else if (dbType == DbType.Sqlite)
            {
                File.Copy(text.Split('=')[1], fullFileName);
            }
        }

        public static void ShrinkDataBase(int? commandTimeout = null)
        {
            string connectionString = (DbAndApiAuthConfig.Config.New ? DbAndApiAuthConfig.Config.DbConnectionString : ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString);
            DbType dbType = (DbAndApiAuthConfig.Config.New ? DbAndApiAuthConfig.Config.DbType : DbType.Sqlite);
            SqlSugarClient sqlSugarClient = new SqlSugarClient(new ConnectionConfig
            {
                ConnectionString = connectionString,
                DbType = dbType,
                IsAutoCloseConnection = true
            });
            if (dbType == DbType.SqlServer)
            {
                string sql = "DBCC SHRINKDATABASE([" + sqlSugarClient.Ado.Connection.Database + "])";
                sqlSugarClient.Ado.ExecuteCommand(sql);
            }
        }
    }
}
