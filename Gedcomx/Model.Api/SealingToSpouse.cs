namespace Model.Api {
    public class SealingToSpouse {
        public Spouse spouse {
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