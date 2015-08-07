using System;
using System.Collections.Generic;
using FindMyFamilies.Util;
using Gx.Conclusion;
using Gx.Types;
using ProtoBuf;

namespace FindMyFamilies.Data {
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields, AsReferenceDefault = true)]
    public class AncestorsDO {
        public AncestorsDO() {
            Ancestors = new List<SelectListItemDO>();
        }

        public List<SelectListItemDO> Ancestors { get; set; }
        public string Id { get; set; }
        public string Text { get; set; }
        public string errorMessage { get; set; }
    }
}