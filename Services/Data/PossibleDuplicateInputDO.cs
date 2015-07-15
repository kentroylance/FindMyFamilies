using System.Collections.Generic;
using FindMyFamilies.Util;

namespace FindMyFamilies.Data {
    public class PossibleDuplicateInputDO {
        public PossibleDuplicateInputDO() {
            Generation = 2;
            ResearchType = Constants.RESEARCH_TYPE_ANCESTORS;
        }

        public string PersonId {
            get;
            set;
        }

        public string PersonName {
            get;
            set;
        }

        public int Generation {
            get;
            set;
        }

        public int RetrievedRecords {
            get;
            set;
        }

        public int RowsReturned {
            get;
            set;
        }

        public string CurrentPersonId {
            get;
            set;
        }

        public string ResearchType {
            get;
            set;
        }

        public bool IncludePossibleDuplicates {
            get;
            set;
        }

        public bool IncludePossibleMatches {
            get;
            set;
        }

        public int ReportId {
            get;
            set;
        }
    }
}