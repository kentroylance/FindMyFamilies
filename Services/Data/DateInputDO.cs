using System.Collections.Generic;
using FindMyFamilies.Util;

namespace FindMyFamilies.Data {
    public class DateInputDO {
        public DateInputDO() {
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

        public bool Empty {
            get;
            set;
        }

        public bool Invalid {
            get;
            set;
        }

        public bool InvalidFormat {
            get;
            set;
        }

        public bool Incomplete {
            get;
            set;
        }

        public int ReportId {
            get;
            set;
        }
    }
}