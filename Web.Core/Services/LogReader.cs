using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace FindMyFamilies.Web {
    public class LogReader {
        public LogReader() {
            LogPathsLoaded = new List<string>();
            LogXmlFiles = new List<string>();
            LogEntries = new List<LogEntry>();
            GroupedLogEntries = new List<LogGrouping>();
        }

        public List<string> LogPathsLoaded {
            get;
            set;
        }

        public List<string> LogXmlFiles {
            get;
            set;
        }

        public List<LogEntry> LogEntries {
            get;
            set;
        }

        public List<LogGrouping> GroupedLogEntries {
            get;
            set;
        }

        public void LoadLogFolder(string environmentName, string logFolder) {
            //Remove Existing LogXmlFiles Entries from folder
            List<string> existingLogXmlFiles = LogXmlFiles.Where(x => x.StartsWith(logFolder)).ToList();
            foreach (string existingLogXmlFile in existingLogXmlFiles) {
                LogXmlFiles.Remove(existingLogXmlFile);
            }

            //Remove Existing LogEntries Entries from folder
            List<LogEntry> existingLogEntries = LogEntries.Where(x => x.FilePath.StartsWith(logFolder)).ToList();
            foreach (LogEntry existingLogEntry in existingLogEntries) {
                LogEntries.Remove(existingLogEntry);
            }

            //Clear The GroupedLogEntries
            GroupedLogEntries = new List<LogGrouping>();

            //Read in the new folder 
            string[] xmlfileList = ReadLogsInFolder(logFolder);
            foreach (string xmlFile in xmlfileList) {
                LogEntries.Add(ReadLogXmlFile(xmlFile, environmentName, logFolder));
            }
            LogEntries = LogEntries.OrderByDescending(x => x.Time).ToList();
            GroupLogEntries();

            //Mark Log Folder as loaded
            LogPathsLoaded.Add(logFolder);
            LogPathsLoaded = LogPathsLoaded.Distinct().ToList();
        }

        //-- Get Logs In Folder

        public string[] ReadLogsInFolder(string logFolder) {
            if (Directory.Exists(logFolder)) {
                string[] xmlfileList = Directory.GetFiles(logFolder);
                LogXmlFiles.AddRange(xmlfileList);

                return xmlfileList;
            }

            return new string[] {};
        }

        //-- Read Logs

        public LogEntry ReadLogXmlFile(string xmlLogFileLocation, string environmentName, string logFolder) {
            var xDoc = new XmlDocument();
            xDoc.Load(xmlLogFileLocation);

            LogEntry logEntry = ReadErrorNodeAttributes(xDoc);
            logEntry.FilePath = xmlLogFileLocation;

            XmlNode serverVariablesNode = xDoc.SelectNodes("error/serverVariables")[0];
            logEntry.ServerVariables = ReadDetailsNodeCollection(serverVariablesNode);

            XmlNode queryStringsNode = xDoc.SelectNodes("error/queryString")[0];
            logEntry.QueryStrings = ReadDetailsNodeCollection(queryStringsNode);

            XmlNode cookiesNode = xDoc.SelectNodes("error/cookies")[0];
            logEntry.Cookies = ReadDetailsNodeCollection(cookiesNode);

            logEntry.Url = logEntry.ServerVariables.ContainsKey("URL") ? logEntry.ServerVariables["URL"] : "";
            logEntry.EnvironmentName = environmentName;
            logEntry.LogDirectory = logFolder;

            return logEntry;
        }

        private static LogEntry ReadErrorNodeAttributes(XmlDocument xDoc) {
            XmlNodeList xmlNodeList = xDoc.SelectNodes("error");
            if (xmlNodeList != null) {
                XmlNode errorNode = xmlNodeList[0];
                var logEntry = new LogEntry {
                    ErrorId = errorNode.Attributes["errorId"].GetValue(), Application = errorNode.Attributes["application"].GetValue(), Host = errorNode.Attributes["host"].GetValue(),
                    ErrorType = errorNode.Attributes["type"].GetValue(), Message = errorNode.Attributes["message"].GetValue(), Source = errorNode.Attributes["source"].GetValue(),
                    Detail = errorNode.Attributes["detail"].GetValue(), User = errorNode.Attributes["user"].GetValue(), Time = errorNode.Attributes["time"].GetValue().Parse<DateTime>(),
                    StatusCode = errorNode.Attributes["statusCode"].GetValue()
                };
                return logEntry;
            }
            return new LogEntry();
        }

        private Dictionary<string, string> ReadDetailsNodeCollection(XmlNode xNode) {
            var serverVariables = new Dictionary<string, string>();
            if (xNode != null) {
                foreach (XmlNode itemNode in xNode.ChildNodes) {
                    string key = itemNode.Attributes["name"].GetValue();
                    string value = itemNode.ChildNodes[0].Attributes["string"].GetValue();

                    serverVariables.Add(key, value);
                }
            }

            return serverVariables;
        }

        //-- Group Logs

        public void GroupLogEntries() {
            var groupedEntries = LogEntries.GroupBy(x => new {x.Detail, x.Url});
            foreach (var entries in groupedEntries) {
                LogEntry lastEntry = entries.First();

                GroupedLogEntries.Add(new LogGrouping {
                    LastErrorID = lastEntry.ErrorId, ErrorType = lastEntry.ErrorType, ErrorMessage = lastEntry.Message, //.MaxLength(500),
                    ErrorDetail = lastEntry.Detail, Url = lastEntry.Url, ErrorCount = entries.Count(), LastReport = entries.Max(x => x.Time.ToString("hh:mm tt | MM/dd/yyyy")),
                    ServerVariables = lastEntry.ServerVariables, QueryStrings = lastEntry.QueryStrings, Cookies = lastEntry.Cookies,
                    LogEntries = entries.Select(x => new LogEntry {Time = x.Time, ErrorId = x.ErrorId, EnvironmentName = x.EnvironmentName}).ToList()
                });
            }
        }

        //-- Clear Logs

        public void ClearLogEntriesInGroup(string lastErrorId) {
            LogEntry errorEntry = LogEntries.First(x => x.ErrorId == lastErrorId);

            string environmentName = errorEntry.EnvironmentName;
            string logDirectory = errorEntry.LogDirectory;

            if (errorEntry != null) {
                List<LogEntry> logEntries = LogEntries.Where(x => x.Detail == errorEntry.Detail).ToList();
                foreach (LogEntry logEntry in logEntries) {
                    File.Delete(logEntry.FilePath);
                    LogEntries.Remove(logEntry);
                }
            }
            LoadLogFolder(environmentName, logDirectory);
        }

        public void RemoveEntries(string logFolder) {
            LogEntry errorEntry = LogEntries.First();

            string environmentName = errorEntry.EnvironmentName;
            string logDirectory = errorEntry.LogDirectory;

            if (errorEntry != null) {
                foreach (LogEntry logEntry in LogEntries) {
                    File.Delete(logEntry.FilePath);
                }
            }
            LogEntries.Clear();
            LoadLogFolder(environmentName, logDirectory);
       }
    }
}