namespace Model.Api {
    public class Confirmation {
        public Date3 date {
            get;
            set;
        }

        public Temple2 temple {
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