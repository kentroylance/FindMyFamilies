using System;
using ProtoBuf;

namespace FindMyFamilies.Data {

	/// <summary>
	/// Summary description for ListDO.
	/// </summary>
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields, AsReferenceDefault = true)]	
    public class SelectListItemDO {

        public SelectListItemDO() {
        } 

        public SelectListItemDO(string id, string text) {
            this.id = id;
            this.text = text;
        } 
		public string id { get; set; }
        public string text { get; set; }
	}
}
