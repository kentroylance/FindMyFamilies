using System;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace FindMyFamilies.Util {
    /// <summary>
    /// Summary description for FilePathHelper.
    /// </summary>
    public class FilePathHelper {
        public FilePathHelper() {
        }

        public static string GetTempPath() {
            Uri uri = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase));
            string path = uri.AbsolutePath + "/Temp";
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        public static string GetErrorLoggerPath() {
            Uri uri = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase));
            string path = uri.AbsolutePath.Replace("bin/", "") + "/ErrorLogs";
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        public static int tempFileCount() {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(GetTempPath());
            int count = dir.GetFiles().Length;
            return dir.GetFiles().Length;
        }

        public static int errorLoggerFileCount() {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(GetErrorLoggerPath());
            int count = dir.GetFiles().Length;
            return dir.GetFiles().Length;
        }

        public static bool exceedsFileCount() {
            Uri uri = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase));
            string path = uri.AbsolutePath;
            path = path.Replace("bin", "");
            path = path + "ErrorLogs";
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(path);
            int count = 0;
            if (dir.Exists) {
                count = dir.GetFiles().Length;
                if (count > 100) {
                    foreach (FileInfo file in dir.GetFiles()) {
                        file.Delete();
                    }
                }
            }
            return (count > 100);
        }
    }
}