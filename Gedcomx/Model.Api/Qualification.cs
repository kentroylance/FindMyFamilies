using System.Collections.Generic;

namespace Model.Api {
    public class Qualification {
        public string gender {
            get;
            set;
        }

        public List<Event> events {
            get;
            set;
        }

        public Name name {
            get;
            set;
        }
    }
}