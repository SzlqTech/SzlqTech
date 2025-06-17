using System;
using System.Collections.Generic;
using System.Configuration;

namespace SzlqTech.Common.Helper
{
    public static  class XmlConfigHelper
    {
        public static void Save(string name,string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[name].Value = value;            
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        public static string GetValue(string name)
        {
       
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings[name] != null)
            {
                return config.AppSettings.Settings[name].Value;
            }
            return string.Empty;
        }
    }
}
