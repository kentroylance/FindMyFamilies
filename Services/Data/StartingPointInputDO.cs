using System.Collections.Generic;
using FindMyFamilies.Util;

namespace FindMyFamilies.Data {
    public class StartingPointInputDO {
        public StartingPointInputDO() {
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

        public int Generation { get; set; }
        public bool NonMormon { get; set; }
        public bool Born18101850 { get; set; }
        public bool LivedInUSA { get; set; }
        public bool NeedOrdinances { get; set; }
        public bool Hints { get; set; }
        public bool Clues { get; set; }
        public bool Sources { get; set; }
        public bool Duplicates { get; set; }
        public int ReportId { get; set; }

    }
}