using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogTest
{
    public class LoggerHelper
    {
        Logger logger = null;
        bool traceEnabled = true;

        public LoggerHelper(string name)
        {
            logger = CreateLogger(name);
        }

        public void Log(string msg)
        {
            logger.Warn(msg);
            if (traceEnabled)
                logger.Trace(msg);
        }

        private static Logger CreateLogger(string name)
        {
            string layoutDebug = "${longdate} - ${level:uppercase=true}: ${message}. ${exception:format=ToString}";
            string layoutTrace = "${longdate} - ${level:uppercase=true} - thread[${threadid}] ${callsite}, Line ${callsite-linenumber}: ${message}. ${exception:format=ToString}";

            string filenameDebug = "logs/{0}.${{shortdate}}.log";
            string filenameTrace = "logs/{0}.TRACE.${{shortdate}}.log";

            // Create Targets
            FileTarget targetDebug = new FileTarget();
            targetDebug.Name = name;
            targetDebug.FileName = string.Format(filenameDebug, name);
            targetDebug.Layout = layoutDebug;
            targetDebug.ReplaceFileContentsOnEachWrite = false;

            FileTarget targetTrace = new FileTarget();
            targetTrace.Name = name;
            targetTrace.FileName = string.Format(filenameTrace, name);
            targetTrace.Layout = layoutTrace;
            targetTrace.ReplaceFileContentsOnEachWrite = false;

            // Create Rules
            LoggingRule ruleDebug = new LoggingRule("Debug");
            ruleDebug.LoggerNamePattern = "*";
            ruleDebug.SetLoggingLevels(LogLevel.Warn, LogLevel.Fatal);
            ruleDebug.Targets.Add(targetDebug);

            LoggingRule ruleTrace = new LoggingRule("Trace");
            ruleTrace.LoggerNamePattern = "*";
            ruleTrace.SetLoggingLevels(LogLevel.Trace, LogLevel.Info);
            ruleTrace.Targets.Add(targetTrace);

            // Create Configuration
            LoggingConfiguration config = new LoggingConfiguration();
            config.AddTarget(name, targetDebug);
            config.AddTarget(name, targetTrace);
            config.LoggingRules.Add(ruleDebug);
            config.LoggingRules.Add(ruleTrace);

            LogFactory factory = new LogFactory();
            factory.Configuration = config;
            return factory.GetCurrentClassLogger();
        }

        public static Logger CreateDebugLogger(string name)
        {
            //string LogEntryLayout = "${ date:format=dd.MM.yyyy HH\\:mm\\:ss.fff} thread[${threadid}] ${logger} (${level:uppercase=true}): ${message}. ${exception:format=ToString}",
            string LogEntryLayout = "${ date:format=dd.MM.yyyy HH\\:mm\\:ss.fff}${level:uppercase=true}: ${message}. ${exception:format=ToString}";
            string logFileLayout = "logs/{0}.DEBUG.${{shortdate}}.log";

            // Create Targets
            FileTarget target = new FileTarget();
            target.Name = name;
            target.ReplaceFileContentsOnEachWrite = false;
            target.FileName = string.Format(logFileLayout, name);
            target.Layout = LogEntryLayout;

            // Create Rules
            LoggingRule ruleInfo = new LoggingRule("Debug");
            ruleInfo.LoggerNamePattern = "*";
            ruleInfo.SetLoggingLevels(LogLevel.Debug, LogLevel.Fatal);
            ruleInfo.Targets.Add(target);

            // Create Configuration
            LoggingConfiguration config = new LoggingConfiguration();
            config.AddTarget(name, target);
            config.LoggingRules.Add(ruleInfo);

            LogFactory factory = new LogFactory();
            factory.Configuration = config;
            return factory.GetCurrentClassLogger();
        }

        public static Logger CreateTraceLogger(string name)
        {
            string LogEntryLayout = "${ date:format=dd.MM.yyyy HH\\:mm\\:ss.fff} thread[${threadid}] ${logger} (${level:uppercase=true}): ${message}. ${exception:format=ToString}";
            string logFileLayout = "logs/{0}.TRACE.${{shortdate}}.log";

            // Create Targets
            FileTarget target = new FileTarget();
            target.Name = name;
            target.ReplaceFileContentsOnEachWrite = false;
            target.FileName = string.Format(logFileLayout, name);
            target.Layout = LogEntryLayout;

            // Create Rules
            LoggingRule ruleInfo = new LoggingRule("Debug");
            ruleInfo.LoggerNamePattern = "*";
            ruleInfo.SetLoggingLevels(LogLevel.Trace, LogLevel.Fatal);
            ruleInfo.Targets.Add(target);

            // Create Configuration
            LoggingConfiguration config = new LoggingConfiguration();
            config.AddTarget(name, target);
            config.LoggingRules.Add(ruleInfo);

            LogFactory factory = new LogFactory();
            factory.Configuration = config;
            return factory.GetCurrentClassLogger();
        }
    }
}
