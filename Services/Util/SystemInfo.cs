using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Web;

namespace FindMyFamilies.Util {
    /// <summary>
    ///     Summary description for SystemInfo.
    /// </summary>
    public class SystemInfo {
        private static SystemInfo instance;
        private static readonly object syncLock = new object();

        private static string m_ImagePath;
        private static string m_ImagePathSystem;
        private static string m_ImagePathApp;

        private static string m_ResourcePath;

        private static string m_ServerPath;
        private static string m_ApplicationName;

        private static string m_ExecutingPath;
        private static string m_ExecutingAssembly;

        private static bool m_ExecutingWindowsStandaloneExe;
        private static bool m_Server;
        private static bool m_Development;
        private static bool m_WindowsClient;
        private static bool m_ExecutingService;
        private static bool m_ExecutingWebApp;
        private static bool m_ExecutingTest;


        public SystemInfo() {
            string processName = Process.GetCurrentProcess().ProcessName;
            if (processName.Equals("ProcessInvocation")) {
                m_ExecutingAssembly = Assembly.GetExecutingAssembly().CodeBase;
                if (m_ExecutingAssembly.IndexOf("Tests") > 0) {
                    m_ExecutingTest = true;
                    m_ApplicationName = m_ExecutingAssembly.Substring(m_ExecutingAssembly.IndexOf("C:/Dev/Source/") + 7,
                        m_ExecutingAssembly.IndexOf("Tests") - m_ExecutingAssembly.IndexOf("C:/Dev/Source/") - 8);
                    m_ExecutingPath =
                        Path.GetDirectoryName(m_ExecutingAssembly.Substring(0, m_ExecutingAssembly.IndexOf("Tests")).Replace(@"file:///", "")).ToLower
                            () + Path.DirectorySeparatorChar;
                } else {
                    m_ExecutingPath = Path.GetDirectoryName(m_ExecutingAssembly.Replace(@"file:///", "")).ToLower() + Path.DirectorySeparatorChar;
                }
                //if ("file:///C:/Dev/Source/FindMyFamilies/Tests/bin/Debug/FindMyFamiliesCommon.DLL"
                string test = "";
            } else if (processName.Equals("WebDev.WebServer40")) {
                m_ExecutingAssembly = HttpContext.Current.Request.ServerVariables["APPL_PHYSICAL_PATH"];
                m_ApplicationName = "FindMyFamilies";
                m_ExecutingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase.Replace(@"file:///", "")).ToLower() +
                    Path.DirectorySeparatorChar;
//
//                m_ApplicationName = m_ExecutingAssembly.Substring(m_ExecutingAssembly.IndexOf("Web") + 4,
//                    m_ExecutingAssembly.Length - (m_ExecutingAssembly.IndexOf("Web") + 4) - 1);
//                m_ApplicationName = m_ApplicationName.Replace("Web", "");
//                m_ExecutingPath = m_ExecutingAssembly;
            } else {
                m_ExecutingAssembly = Assembly.GetExecutingAssembly().CodeBase;
                m_ApplicationName = "FindMyFamilies";
                m_ExecutingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase.Replace(@"file:///", "")).ToLower() +
                    Path.DirectorySeparatorChar;
            }

            if (m_ExecutingAssembly.IndexOf("Web") > 0) {
                m_ExecutingWebApp = true;
                m_ServerPath = m_ExecutingPath;
            }
            if ((m_ExecutingAssembly.IndexOf("Remoting") > 0) || ((m_ExecutingAssembly.IndexOf("MSMQService") > 0))) {
                m_ExecutingService = true;
                m_ServerPath = m_ExecutingPath;
            }
            if (m_ExecutingPath.IndexOf("\\dev\\source\\") > 0) {
                m_Development = true;
            }
            if (m_ExecutingPath.IndexOf("\\program files\\") > 0) {
                m_WindowsClient = true;
                m_ExecutingWindowsStandaloneExe = true;
            }
        }

        public static SystemInfo Instance {
            get {
                // Support multithreaded applications through
                // 'Double checked locking' pattern which (once
                // the instance exists) avoids locking each
                // time the method is invoked
                lock (syncLock) {
                    if (instance == null) {
                        instance = new SystemInfo();
                    }

                    return instance;
                }
            }
        }

        public bool Debugging {
            get {
                return m_Development;
            }
        }

        public bool Development {
            get {
                return m_Development;
            }
        }

        public string ExecutingPath {
            get {
                return m_ExecutingPath;
            }
        }

        public string ApplicationName {
            get {
                return m_ApplicationName;
            }
        }

        public string ExecutingAssembly {
            get {
                return m_ExecutingAssembly;
            }
        }

        public bool ExecutingService {
            get {
                return m_ExecutingService;
            }
        }

        public bool ExecutingWebApp {
            get {
                return m_ExecutingWebApp;
            }
        }

        public bool ExecutingTest {
            get {
                return m_ExecutingTest;
            }
        }

        public string ResourcePath {
            get {
                if (m_ResourcePath == null) {
//                    var path = System.Reflection.Assembly.GetExecutingAssembly ().Location;
//                    var name = System.IO.Path.GetFileName (path);
//                    m_ResourcePath = path.Replace(name, "");
                    m_ResourcePath = m_ExecutingPath;
                }
                return m_ResourcePath;
            }
        }
    }
}