using Model.Api;

namespace FindMyFamilies.Data {
    public class OrdinanceDO {
        public int statusCode {
            get;
            set;
        }

        public string statusMessage {
            get;
            set;
        }

        public string Id {
            get;
            set;
        }

        public string Fullname {
            get;
            set;
        }

        public Baptism Baptism {
            get;
            set;
        }

        public Confirmation Confirmation {
            get;
            set;
        }

        public Initiatory Initiatory {
            get;
            set;
        }

        public Endowment Endowment {
            get;
            set;
        }
        public SealingToParent SealedToParent {
            get;
            set;
        }
        public SealingToSpouse SealedToSpouse {
            get;
            set;
        }
    }
}