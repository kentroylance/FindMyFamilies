using System;
using System.Collections.Generic;
using FindMyFamilies.Util;
using Gx.Conclusion;
using Gx.Types;
using ProtoBuf;

namespace FindMyFamilies.Data {
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields, AsReferenceDefault = true)]
    public class PersonsDO {
        private Dictionary<string, PersonDO> _Persons;

        public PersonsDO() {
        }

        public Dictionary<string, PersonDO> Persons {
            get {
                return _Persons;
            }
            set {
                _Persons = value;
            }
        }
    }
}