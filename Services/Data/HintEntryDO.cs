using System.Collections;
using System.Collections.Generic;
using Model.Api;

namespace FindMyFamilies.Data {
    public class HintEntryDO {
        public HintEntryDO() {
            Persons = new Dictionary<string, PersonDO>();
            Links = new List<string>();
        }

        public string Id {
            get;
            set;
        }

        public string Fullname {
            get;
            set;
        }

        public string Updated {
            get;
            set;
        }

        public double Score {
            get;
            set;
        }

        public List<string> Links {
            get;
            set;
        }

        public Dictionary<string, PersonDO> Persons {
            get;
            set;
        }
    }
}