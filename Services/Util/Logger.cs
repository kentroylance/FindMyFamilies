using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace FindMyFamilies.Util {
    public class Logger : ILogger {
        private static ILog log;
        private static bool isInitialized;

        public Logger(Type type) {
            if (!isInitialized) {
                var uri = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase));
                var fileMap = new ExeConfigurationFileMap {
                    ExeConfigFilename = Path.Combine(uri.LocalPath, "log4net.config")
                };
                XmlConfigurator.ConfigureAndWatch(new FileInfo(fileMap.ExeConfigFilename));
                isInitialized = true;
                GlobalContext.Properties["host"] = Environment.MachineName;
            }
            log = LogManager.GetLogger(type);
        }

        public void Enter(string methodName) {
            if (log.IsInfoEnabled) {
                log.Info(string.Format(CultureInfo.InvariantCulture, "Entering Method {0}", methodName));
            }
        }

        public void Leave(string methodName) {
            if (log.IsInfoEnabled) {
                log.Info(string.Format(CultureInfo.InvariantCulture, "Leaving Method {0}", methodName));
            }
        }

        public void Error(string message) {
            if (log.IsErrorEnabled) {
                log.Error(string.Format(CultureInfo.InvariantCulture, "{0}", message));
            }
        }

        public void Warning(string message) {
            if (log.IsWarnEnabled) {
                log.Warn(string.Format(CultureInfo.InvariantCulture, "{0}", message));
            }
        }

        public void Info(string message) {
            if (log.IsInfoEnabled) {
                log.Info(string.Format(CultureInfo.InvariantCulture, "{0}", message));
            }
        }

//        public static void Setup() {
//            var hierarchy = (Hierarchy) LogManager.GetRepository();
//
//            var patternLayout = new PatternLayout();
//            patternLayout.ConversionPattern = "%date [%thread] %-5level %logger - %message%newline";
//            patternLayout.ActivateOptions();
//
//            var roller = new RollingFileAppender();
//            roller.AppendToFile = false;
//            roller.File = @"Logs\EventLog.txt";
//            roller.Layout = patternLayout;
//            roller.MaxSizeRollBackups = 5;
//            roller.MaximumFileSize = "1GB";
//            roller.RollingStyle = RollingFileAppender.RollingMode.Size;
//            roller.StaticLogFileName = true;
//            roller.ActivateOptions();
//            hierarchy.Root.AddAppender(roller);
//
//            var memory = new MemoryAppender();
//            memory.ActivateOptions();
//            hierarchy.Root.AddAppender(memory);
//
//            hierarchy.Root.Level = Level.Error;
//            hierarchy.Configured = true;
//        }

        public void Debug(string message) {
            if (log.IsDebugEnabled) {
                log.Debug(string.Format(CultureInfo.InvariantCulture, "{0}", message));
            }
        }

        public void Error(string message, Exception exception) {
            if (log.IsErrorEnabled) {
                log.Error(string.Format(CultureInfo.InvariantCulture, "{0}", message), exception);
            }
        }

        public void Error(Exception exception) {
            if (log.IsErrorEnabled) {
                log.Error(string.Format(CultureInfo.InvariantCulture, "{0}", exception.Message), exception);
            }
        }
    }
}