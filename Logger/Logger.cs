using System;
using NLog;
namespace Logger
{
    public static class Logger
    {

        private static readonly NLog.Logger nLogger = NLog.LogManager.GetCurrentClassLogger();

        public static void LogMessageInfo(string toLog)
        {
            if (!String.IsNullOrWhiteSpace(toLog))
            {
                nLogger.Info(toLog);
            }
        }

        public static void LogException(Exception ex, string message)
        {
            if (ex != null)
            {
                nLogger.Error(ex, message);
            }

        }

    }
}
