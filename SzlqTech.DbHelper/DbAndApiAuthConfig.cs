using SqlSugar;
using System.Configuration;
using SzlqTech.Common.Helper;

namespace SzlqTech.DbHelper
{
    [Serializable]
    public class DbAndApiAuthConfig
    {
        private static DbAndApiAuthConfig config;

        public static DbAndApiAuthConfig Config
        {
            get
            {
                if (config == null)
                {
                    config = BinHelper.Load<DbAndApiAuthConfig>();
                }

                return config ?? (config = new DbAndApiAuthConfig());
            }
        }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Key { get; set; }

        public DbType DbType { get; set; } = DbType.SqlServer;


        public bool New { get; set; }

        public string DbConnectionString { get; set; } = ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;


        public static void LoadNewConfig()
        {
            config = BinHelper.Load<DbAndApiAuthConfig>();
        }
    }
}
