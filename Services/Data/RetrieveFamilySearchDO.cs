using FindMyFamilies.Util;
using ProtoBuf;

namespace FindMyFamilies.Data {
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields, AsReferenceDefault = true)]
    public class RetrieveFamilySearchDO {
        public RetrieveFamilySearchDO() {
            Generation = 2;
            ResearchType = Constants.RESEARCH_TYPE_ANCESTORS;
        }

        public string Title {
            get;
            set;
        }

        public string PersonId {
            get;
            set;
        }

        public string Name {
            get;
            set;
        }

        public int Generation {
            get;
            set;
        }

        public string ResearchType {
            get;
            set;
        }

        public int StartAt {
            get;
            set;
        }

        public bool IncludeMaidenName {
            get;
            set;
        }

        public bool IncludeMiddleName {
            get;
            set;
        }

        public bool IncludePlace {
            get;
            set;
        }

        public bool AddChildren {
            get;
            set;
        }

        public int YearRange {
            get;
            set;
        }

        public int RowsReturned {
            get;
            set;
        }

        public string PersonType {
            get;
            set;
        }

    }
}