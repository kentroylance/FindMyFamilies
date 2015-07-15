using System;
using System.Collections;
using System.Collections.Specialized;
using FindMyFamilies.Util;

namespace FindMyFamilies.Data {
	/// <summary>
	/// Summary description for 
	/// </summary>
	public class SecurityDO : ICloneable {
		public const string MEMBER = "Member";
		public const string GROUPS = "Groups";
		public const string RIGHTS = "Rights";
		public const string MEMBER_GROUPS = "MemberGroups";
		
		private string m_LoginPassword;
		private string m_LoginID;
		private IDictionary m_Data;
		private bool m_LoggedInLocally;

		public SecurityDO() : base() {
		}

		public SecurityDO(string loginID, string loginPassword) {
			this.LoginID = loginID;
			this.LoginPassword = loginPassword;
		}

		public SecurityDO(string loginID, string loginPassword, int loginMemberID) {
			this.LoginID = loginID;
			this.LoginPassword = loginPassword;
//			this.LoginMemberID = loginMemberID;
		}

		public IDictionary Data {
			get {
				if (m_Data == null) {
					m_Data = new HybridDictionary();
				}
				return m_Data;
			}
			set {
				m_Data = value;
			}
		}

		public IDictionary Groups {
			get {
				if (Data[GROUPS] == null) {
					Data[GROUPS] = new Hashtable();
				}
				return (IDictionary) Data[GROUPS];
			}
			set {
				if (!Data.Contains(GROUPS)) {
					Data.Add(GROUPS, value);
				} else {
					Data[GROUPS] = value;
				}
			}
		}

		public IDictionary Rights {
			get {
				if (Data[RIGHTS] == null) {
					Data[RIGHTS] = new Hashtable();
				}
				return (IDictionary) Data[RIGHTS];
			}
			set {
				if (!Data.Contains(RIGHTS)) {
					Data.Add(RIGHTS, value);
				} else {
					Data[RIGHTS] = value;
				}
			}
		}

		public bool HasGroup(object groupValue) {
			return (this.Groups[groupValue] != null);
		}

		// new int[] {Constants.ROLE_CORE_TEAM, Constants.ROLE_ADMIN}
		public bool HasGroup(int[] groups) {
			bool hasGroup = false;

			for (int i = 0; i < groups.Length; i++) {
				if (this.HasGroup(groups[i])) {
					hasGroup = true;
					break;
				}
			}
			return hasGroup;
		}

		public bool HasRight(string rightKey) {
			bool hasRight = false;
			foreach (string rightsKey in Rights.Keys ) {
				if (rightsKey.StartsWith(rightKey)){
					hasRight = true;
					break;
				}
			}
			return hasRight;
		}

		public void AddToRights(ICollection rights) {
			IEnumerator enumerator = ((IDictionary) rights).Keys.GetEnumerator();
			string rightKey = null;
			Hashtable rightsTable = (Hashtable)this.Rights;
			while (enumerator.MoveNext()) {
				rightKey = (string) enumerator.Current;
				if(!rightsTable.ContainsKey(rightKey) ){
					rightsTable.Add(rightKey, null);
				}
			}
			this.Rights = rightsTable;
		}

		public void DeleteGroup(object groupValue) {
			if (this.HasGroup(groupValue)) {
				this.Groups.Remove(groupValue);
			}
		}

		public void AddGroup(object groupValue) {
			if (!this.HasGroup(groupValue)) {
				this.Groups.Add(groupValue, groupValue);
			}
		}

		public void AddGroup(bool isGroupChecked, object groupValue) {
			if (isGroupChecked) {
				this.AddGroup(groupValue);
			} else {
				this.DeleteGroup(groupValue);
			}
		}

		public bool IsAdmin() {
			return (this.Groups[Constants.GROUP_ADMIN] != null);
		}

		public string LoginID {
			get {
				return m_LoginID;
			}
			set {
				m_LoginID = value;
			}
		}

		public string LoginPassword {
			get {
				return m_LoginPassword;
			}
			set {
				m_LoginPassword = value;
			}
		}

//		public int LoginMemberID {
//			get {
//				return Member.MemberID;
//			}
//			set {
//				Member.MemberID = value;
//			}
//		}

		public IList Member_Groups {
			get {
				if (Data[MEMBER_GROUPS] == null) {
					Data[MEMBER_GROUPS] = new ArrayList();
				}
				return (IList) Data[MEMBER_GROUPS];
			}
			set {
				if (!Data.Contains(MEMBER_GROUPS)) {
					Data.Add(MEMBER_GROUPS, value);
				} else {
					Data[MEMBER_GROUPS] = value;
				}
			}
		}

		public MemberDO Member {
			get {
				if (Data[MEMBER] == null) {
					Data[MEMBER] = new MemberDO();
				}
				return (MemberDO) Data[MEMBER];
			}
			set {
				if (value != null) {
					MemberDO member = (MemberDO) value;
				}
				if (!Data.Contains(MEMBER)) {
					Data.Add(MEMBER, value);
				} else {
					Data[MEMBER] = value;
				}
			}
		}

		public bool LoggedInLocally {
			get {
				return m_LoggedInLocally;
			}
			set {
				m_LoggedInLocally = value;
			}
		}

		public virtual object Clone() {
			return MemberwiseClone();
		}
	}
}