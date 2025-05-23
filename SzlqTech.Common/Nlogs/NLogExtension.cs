using NLog;

namespace SzlqTech.Common.Nlogs
{
    public static class NLogExtension
    {
        public static EventHandler? LogEventHandler;

        private static void OnLogEventHandler(LoggerLevel loggerLevel, string message)
        {
            if (LogEventHandler != null)
            {
                BaseLogger sender = new BaseLogger(loggerLevel, message);
                LogEventHandler?.Invoke(sender, null);
            }
        }

        public static void DebugHandler(this Logger logger, string message)
        {
            OnLogEventHandler(LoggerLevel.Debug, message);
            logger.Debug(message);
        }

        public static void DebugHandler(this Logger logger, string message, params object[] args)
        {
            OnLogEventHandler(LoggerLevel.Debug, message);
            logger.Debug(message, args);
        }

        public static void DebugHandler(this Logger logger, Exception exception, string message)
        {
            OnLogEventHandler(LoggerLevel.Debug, message);
            logger.Debug(exception, message);
        }

        public static void DebugHandler(this Logger logger, Exception exception)
        {
            OnLogEventHandler(LoggerLevel.Debug, exception.Message);
            logger.Debug(exception);
        }

        public static void InfoHandler(this Logger logger, string message)
        {
            OnLogEventHandler(LoggerLevel.Info, message);
            logger.Info(message);
        }

        public static void InfoHandler(this Logger logger, string message, params object[] args)
        {
            OnLogEventHandler(LoggerLevel.Info, message);
            logger.Info(message, args);
        }

        public static void InfoHandler(this Logger logger, Exception exception, string message)
        {
            OnLogEventHandler(LoggerLevel.Info, message);
            logger.Info(exception, message);
        }

        public static void InfoHandler(this Logger logger, Exception exception)
        {
            OnLogEventHandler(LoggerLevel.Info, exception.Message);
            logger.Info(exception);
        }

        public static void WarnHandler(this Logger logger, string message)
        {
            OnLogEventHandler(LoggerLevel.Warn, message);
            logger.Warn(message);
        }

        public static void WarnHandler(this Logger logger, string message, params object[] args)
        {
            OnLogEventHandler(LoggerLevel.Warn, message);
            logger.Warn(message, args);
        }

        public static void WarnHandler(this Logger logger, Exception exception, string message)
        {
            OnLogEventHandler(LoggerLevel.Warn, message);
            logger.Warn(exception, message);
        }

        public static void WarnHandler(this Logger logger, Exception exception)
        {
            OnLogEventHandler(LoggerLevel.Warn, exception.Message);
            logger.Warn(exception);
        }

        public static void ErrorHandler(this Logger logger, string message)
        {
            OnLogEventHandler(LoggerLevel.Error, message);
            logger.Error(message);
        }

        public static void ErrorHandler(this Logger logger, string message, params object[] args)
        {
            OnLogEventHandler(LoggerLevel.Error, message);
            logger.Error(message, args);
        }

        public static void ErrorHandler(this Logger logger, Exception exception, string message)
        {
            OnLogEventHandler(LoggerLevel.Error, message);
            logger.Error(exception, message);
        }

        public static void ErrorHandler(this Logger logger, Exception exception)
        {
            OnLogEventHandler(LoggerLevel.Error, exception.Message);
            logger.Error(exception);
        }

        public static void FatalHandler(this Logger logger, string message)
        {
            OnLogEventHandler(LoggerLevel.Fatal, message);
            logger.Fatal(message);
        }

        public static void FatalHandler(this Logger logger, string message, params object[] args)
        {
            OnLogEventHandler(LoggerLevel.Fatal, message);
            logger.Fatal(message, args);
        }

        public static void FatalHandler(this Logger logger, Exception exception, string message)
        {
            OnLogEventHandler(LoggerLevel.Fatal, message);
            logger.Fatal(exception, message);
        }

        public static void FatalHandler(this Logger logger, Exception exception)
        {
            OnLogEventHandler(LoggerLevel.Fatal, exception.Message);
            logger.Fatal(exception);
        }
    }
}
