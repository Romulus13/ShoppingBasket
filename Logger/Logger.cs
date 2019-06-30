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

    }
}
