using System;
using Model.Api;

namespace FindMyFamilies.Data {
    public class StartingPointDO {
        public bool BornBetween1810and1850 {
            get;
            set;
        }

        public bool BornInUSA {
            get;
            set;
        }

        public bool DiedInUSA {
            get;
            set;
        }

        public bool BaptizedBeforeDied {
            get;
            set;
        }

        public bool Ordinances {
            get;
            set;
        }

        public bool NoBirthDate {
            get;
            set;
        }

        public bool NoBirthPlace {
            get;
            set;
        }

        public bool PossibleDuplicates {
            get;
            set;
        }

        public string PossibleDuplicatesInfo {
            get;
            set;
        }

        public DateTime? BaptismDate {
            get;
            set;
        }

        public bool Hints {
            get;
            set;
        }

        public string HintInfo {
            get;
            set;
        }

        public bool Selected {
            get;
            set;
        }

    }
}