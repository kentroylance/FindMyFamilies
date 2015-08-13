using FindMyFamilies.Util;

namespace FindMyFamilies.Data {
    public class FindCluesInputDO {
        public FindCluesInputDO() {
            Generation = 2;
            ResearchType = Constants.RESEARCH_TYPE_ANCESTORS;
        }

        public string Id {
            get;
            set;
        }

        public string FullName {
            get;
            set;
        }

        public string ResearchType {
            get;
            set;
        }

        public int Generation {
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