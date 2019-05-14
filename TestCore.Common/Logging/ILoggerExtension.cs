using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using log4net.Appender;
using System.Linq;

namespace log4net
{
    public static class ILoggerExtension
    {

        public static void LogInfo(this ILog logger)
        {
            var repository = LogManager.GetRepository("");
            var appenders = repository.GetAppenders();
            var targetApder = appenders.First(p => p.Name == "ShowSqlAppender") as RollingFileAppender;
            targetApder.File = "D:/Media/Logs/Cache/";
            targetApder.ActivateOptions();
        }



    }
}
