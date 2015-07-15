using System.Collections.Generic;

namespace Model.Api {
    public class Persons {
        public object start {
            get;
            set;
        }

        public object end {
            get;
            set;
        }

        public int count {
            get;
            set;
        }

        public List<Person> person {
            get;
            set;
        }
    }
}