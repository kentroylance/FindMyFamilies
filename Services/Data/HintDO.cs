using System.Collections;
using System.Collections.Generic;
using Model.Api;

namespace FindMyFamilies.Data {
    public class HintDO {
        public HintDO()
        {
            Entries = new List<HintEntryDO>();
        }

        public string Id {
            get;
            set;
        }

        public string Fullname {
            get;
            set;
        }


        public int Results {
            get;
            set;
        }

        public double Score {
            get;
            set;
        }

        public bool Matches {
            get;
            set;
        }

        public bool Duplicates {
            get;
            set;
        }

        public string Title {
            get;
            set;
        }

         public List<HintEntryDO> Entries {
            get;
            set;
        }
    }
}