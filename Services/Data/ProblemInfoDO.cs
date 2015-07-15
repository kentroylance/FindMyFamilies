using FindMyFamiles.Services.Data;
using FindMyFamilies.Util;
using ProtoBuf;

namespace FindMyFamilies.Data {
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields, AsReferenceDefault = true)]
    public class ProblemInfoDO {
        private PersonInfoDO _personInfoDO;

        public ProblemInfoDO() {
        }

        public PersonInfoDO PersonInfo {
            get {
                if (_personInfoDO == null) {
                    _personInfoDO = new PersonInfoDO();
                }
                return _personInfoDO;
            }
            set {
                _personInfoDO = value;
            }
        }


        public ValidationDO Validation {
            get;
            set;
        }
    }
}