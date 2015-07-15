namespace Model.Api {
    public class Reservation {
        public Persons persons {
            get;
            set;
        }

        public int statusCode {
            get;
            set;
        }

        public string statusMessage {
            get;
            set;
        }

        public object deprecated {
            get;
            set;
        }

        public string version {
            get;
            set;
        }
    }
}