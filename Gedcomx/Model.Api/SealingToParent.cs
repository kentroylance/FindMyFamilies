using System.Collections.Generic;

namespace Model.Api {
    public class SealingToParent {
        public bool bornInCovenant {
            get;
            set;
        }

        public List<Parent> parent {
            get;
            set;
        }

        public string status {
            get;
            set;
        }

        public bool completed {
            get;
            set;
        }

        public bool reservable {
            get;
            set;
        }

        public bool readyForTrip {
            get;
            set;
        }
    }
}