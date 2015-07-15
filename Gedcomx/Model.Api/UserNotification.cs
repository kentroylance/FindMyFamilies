using System.Collections.Generic;

namespace Model.Api {
    public class UserNotification {
        public string message {
            get;
            set;
        }

        public List<Person2> person {
            get;
            set;
        }

        public List<PersonOrdinance> personOrdinances {
            get;
            set;
        }

        public string code {
            get;
            set;
        }

        public string level {
            get;
            set;
        }
    }
}