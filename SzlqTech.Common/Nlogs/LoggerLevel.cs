using System.ComponentModel;


namespace SzlqTech.Common.Nlogs
{
    public enum LoggerLevel
    {
        [Description("调试")]
        Debug,
        [Description("信息")]
        Info,
        [Description("警告")]
        Warn,
        [Description("错误")]
        Error,
        [Description("致命")]
        Fatal
    }
}
