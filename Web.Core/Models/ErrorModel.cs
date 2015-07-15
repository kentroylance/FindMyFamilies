namespace FindMyFamilies.Web.Models {
    public class ErrorModel {
        public string StatusCode {
            get;
            set;
        }

        public string ReportTitle {
            get;
            set;
        }

        public string ReportBody {
            get;
            set;
        }

        public string CrashReportTitle {
            get;
            set;
        }

        public string BrokenUrl {
            get;
            set;
        }
    }
}