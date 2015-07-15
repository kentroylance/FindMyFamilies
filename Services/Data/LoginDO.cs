namespace FindMyFamilies.Data {
    public class LoginDO {
        public string PersonId {
            get;
            set;
        }

        public string Token {
            get;
            set;
        }

        public string DisplayName {
            get;
            set;
        }

        public string Token24HourExpire {
            get;
            set;
        }

        public string TokenHourExpire {
            get;
            set;
        }

        public string ReportFilePath {
            get;
            set;
        }
    }
}