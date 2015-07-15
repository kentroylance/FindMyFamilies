using System.Collections.Generic;
using Gx.Conclusion;

namespace FindMyFamilies.Data {

    public class FamilyTreeDO {

        private Person _personGX;
        private List<PersonDO> _personDOs;

        public FamilyTreeDO() {
        }

        public Person PersonGx {
            get {
                if (_personGX == null) {
                    _personGX = new Person();
                }
                return _personGX;
            }
            set {
                _personGX = value;
            }
        }

    }
}