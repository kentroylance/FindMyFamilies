namespace Model.Api {
    public class Baptism {
        public Date2 date {
            get;
            set;
        }

        public Temple temple {
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