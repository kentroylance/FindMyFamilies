using System.Collections.Generic;
using FindMyFamilies.Util;

namespace FindMyFamilies.Data {
    public class PlaceInputDO {
        public PlaceInputDO() {
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