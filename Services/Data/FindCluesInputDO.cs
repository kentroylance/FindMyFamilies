namespace FindMyFamilies.Data {
    public class FindCluesInputDO {
        public FindCluesInputDO() {
        }

        public string PersonId {
            get;
            set;
        }

        public string PersonName {
            get;
            set;
        }

        public int SearchCriteria {
            get;
            set;
        }

        public int GapInChildren {
            get;
            set;
        }

        public int AgeLimit {
            get;
            set;
        }

        public int ReportId {
            get;
            set;
        }
    }
}