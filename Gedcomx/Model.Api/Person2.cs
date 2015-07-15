using System.Collections.Generic;

namespace Model.Api {
    public class Person2 {
        public List<object> sealingToParents {
            get;
            set;
        }

        public List<object> sealingToSpouse {
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

        public object requestedRef {
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