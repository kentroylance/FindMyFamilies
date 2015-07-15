using FindMyFamilies.Web.Models;

namespace FindMyFamilies.Data {
    public class ResearchPersonVM {
        public ResearchPersonVM() {
            Generation = 2;
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

    }
}