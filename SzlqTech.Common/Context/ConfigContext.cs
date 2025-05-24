using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SzlqTech.Common.Context
{
    [Serializable]
    public class ConfigContext
    {
        private static string config = "Config";

        private static readonly string view = "View";

        private static readonly string sync = "Sync";

        private static readonly string success = "Success";

        private static readonly string fail = "Fail";

        private static readonly string customLog = "Log";

        private static readonly string ftp = "Ftp";

        //private static readonly string ptsource = CommonText.PrintTemplate + ".btw";

        private static readonly string barTenderTemplate = "BarTenderTemplate";

        private static readonly string delBak = "DelBak";

        public static string Root => Environment.CurrentDirectory;

        public static string Config
        {
            get
            {
                string path = Root + "\\" + config;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return config;
            }
        }

        public static string View
        {
            get
            {
                string path = Root + "\\" + Config + "\\" + view;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return view;
            }
        }

        public static string Sync
        {
            get
            {
                string path = Root + "\\" + sync;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return sync;
            }
        }

        public static string CustomLog
        {
            get
            {
                string path = Root + "\\" + customLog;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return customLog;
            }
        }

        public static string Ftp
        {
            get
            {
                string text = Root + "\\" + ftp;
                if (!Directory.Exists(text))
                {
                    Directory.CreateDirectory(text);
                }

                return text;
            }
        }

        public static string SyncFail
        {
            get
            {
                string text = Root + "\\" + sync + "\\" + fail;
                if (!Directory.Exists(text))
                {
                    Directory.CreateDirectory(text);
                }

                return text;
            }
        }

        public static string SyncSuccess
        {
            get
            {
                string text = Root + "\\" + sync + "\\" + success;
                if (!Directory.Exists(text))
                {
                    Directory.CreateDirectory(text);
                }

                return text;
            }
        }

       //public static string PrinterTemplateSource => Root + "\\" + ptsource;

        public static string BarTenderTemplate
        {
            get
            {
                string text = $"{Root}\\{barTenderTemplate}";
                if (!Directory.Exists(text))
                {
                    Directory.CreateDirectory(text);
                }

                return text;
            }
        }

        public static string DelBak
        {
            get
            {
                string text = Root + "\\" + delBak;
                if (!Directory.Exists(text))
                {
                    Directory.CreateDirectory(text);
                }

                return text;
            }
        }

        public static string RootPath { get; } = Root;


        public static string ConfigPath { get; } = Root + "\\" + Config;


        public static string SyncPath { get; } = Root + "\\" + Sync;


        public static string ViewConfigPath { get; } = Root + "\\" + Config + "\\" + View;


        public static string CustomLogPath { get; } = $"{CustomLog}\\{DateTime.Now:yyyyMM}\\{DateTime.Now:yyyyMMdd}";

    }
}
