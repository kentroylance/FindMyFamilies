using System;
using ProtoBuf;

namespace FindMyFamilies.Data {

	/// <summary>
	/// Summary description for ListDO.
	/// </summary>
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields, AsReferenceDefault = true)]	
    public class ListItemDO {

		private string m_ValueMember;
		private string m_DisplayMember;

		public const string VALUE_MEMBER = "ValueMember";
		public const string DISPLAY_MEMBER = "DisplayMember";


		public ListItemDO() : base() {
			//
			// TODO: Add constructor logic here
			//
		}

		public ListItemDO(string itemValueMember, string itemDisplayMember) {
			this.DisplayMember = itemDisplayMember;
			this.ValueMember = itemValueMember;
		}

		public string DisplayMember {
			get {
				return m_DisplayMember;
			}
			set {
				m_DisplayMember = value;
			}
		}

		public string ValueMember {
			get {
				return m_ValueMember;
			}
			set {
				m_ValueMember = value;
			}
		}

	}
}
