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
                    //config = BinHelper.Load<DbAndApiAuthConfig>();
                    // 读取配置文件中的键值对
                    config = new DbAndApiAuthConfig();
                    string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
                    DbAndApiAuthConfig.config.DbConnectionString = connectionString;
                    Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    DbAndApiAuthConfig.config.Username= configuration.AppSettings.Settings["UserName"].Value??string.Empty;
                    DbAndApiAuthConfig.config.Password = configuration.AppSettings.Settings["Password"].Value ?? string.Empty;
                    DbAndApiAuthConfig.config.New =bool.Parse(configuration.AppSettings.Settings["New"].Value??"false");
                }

                return config ?? (config = new DbAndApiAuthConfig());
            }
        }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Key { get; set; }

        public DbType DbType { get; set; } = DbType.MySql;


        public bool New { get; set; }


        public string DbConnectionString { get; set; }
        // public string DbConnectionString { get; set; } = ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;




        public static void LoadNewConfig()
        {
            config = BinHelper.Load<DbAndApiAuthConfig>();
        }
    }
}
