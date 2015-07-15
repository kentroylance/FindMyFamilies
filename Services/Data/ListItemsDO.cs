using System;
using System.Collections.Generic;
using FindMyFamilies.Util;
using Gx.Conclusion;
using Gx.Types;
using ProtoBuf;

namespace FindMyFamilies.Data {
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields, AsReferenceDefault = true)]
    public class ListItemsDO {
        private Dictionary<string, ListItemDO> _listItems;

        public ListItemsDO() {
        }

        public Dictionary<string, ListItemDO> ListItems {
            get {
                return _listItems;
            }
            set {
                _listItems = value;
            }
        }
    }
}