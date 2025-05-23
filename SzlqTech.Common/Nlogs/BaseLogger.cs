using System.ComponentModel;
using SzlqTech.Common.Extensions;

namespace SzlqTech.Common.Nlogs
{
    public class BaseLogger
    {
        public LoggerLevel LoggerLevel { get; set; }

        public string Message { get; set; }

        public object[] Args { get; set; }

        public BaseLogger(LoggerLevel loggerLevel, string message, params object[] args)
        {
            LoggerLevel = loggerLevel;
            Message = $"{DateTime.Now:HH:mm:ss} {loggerLevel.GetAttributeOfType<DescriptionAttribute>()?.Description} {message}";
            Args = args;
        }
    }
}
