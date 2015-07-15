using FindMyFamilies.Util;
using ProtoBuf;

namespace FindMyFamilies.Data {
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields, AsReferenceDefault = true)]
    public class AnalyzeFamilySearchDO {
        public AnalyzeFamilySearchDO() {
            Include = Constants.RESEARCH_TYPE_ANCESTORS;
            GapInChildren = 3;
            AgeLimit = 20;
            RowsReturned = 10;
        }

        public int SearchCriteria {
            get;
            set;
        }

        public int GapInChildren {
            get;
            set;
        }

        public int ReportId {
            get;
            set;
        }

        public string PersonId {
            get;
            set;
        }

        public string Include {
            get;
            set;
        }

        public int Generation {
            get;
            set;
        }

        public int AgeLimit {
            get;
            set;
        }

        public int RowsReturned {
            get;
            set;
        }

    }
}