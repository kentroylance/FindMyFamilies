using System.Collections.Generic;

namespace Model.Api {
    public class Person {
        public Qualification qualification {
            get;
            set;
        }

        public bool bornInCovenant {
            get;
            set;
        }

        public Baptism baptism {
            get;
            set;
        }

        public Confirmation confirmation {
            get;
            set;
        }

        public Initiatory initiatory {
            get;
            set;
        }

        public Endowment endowment {
            get;
            set;
        }

        public List<SealingToParent> sealingToParents {
            get;
            set;
        }

        public List<SealingToSpouse> sealingToSpouse {
            get;
            set;
        }

        public string @ref {
            get;
            set;
        }

        public object tempId {
            get;
            set;
        }

        public string requestedRef {
            get;
            set;
        }

        public object version {
            get;
            set;
        }

        public object action {
            get;
            set;
        }

        public object role {
            get;
            set;
        }

        public object status {
            get;
            set;
        }

        public object readyForTrip {
            get;
            set;
        }
    }
}