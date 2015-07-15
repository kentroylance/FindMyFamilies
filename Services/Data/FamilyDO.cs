using System.Collections.Generic;

namespace FindMyFamilies.Data {

    public class FamilyDO {

        private int _yearsMarried;

        public FamilyDO() {
            PersonDOs = new List<PersonDO>();
        }

        public PersonDO Husband {
            get;
            set;
        }

        public PersonDO Wife {
            get;
            set;
        }

        public List<PersonDO> PersonDOs {
            get;
            set;
        }

    }
}