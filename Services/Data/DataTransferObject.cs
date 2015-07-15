using System;
using System.Collections;
using System.Collections.Specialized;
using FindMyFamilies.Util;
using ProtoBuf;

namespace FindMyFamilies.Data {
	/// <summary>
	/// Summary description for DataObject.
	/// </summary>
	[ProtoContract]
	public class DataTransferObject : ICloneable {
		// Constants
		public const string FAILED = "failed";
		public const string SUCCESSFUL = "successful";
		public const string PAGING = "paging";
		public const string LANGUAGE = "language_id";

		public const string MEMBER = "Member";
		public const string VERIFY = "Verify";
		public const string LOGGING_IN = "LoggingIn";
		public const string NEXT_IDS = "NextIDs";
		public const string SECURITY = "Security";

		// User Member Declarations
		private int m_LoginMemberID;
		private string m_LoginPassword;
		private string m_LoginID;
		private string m_Language;

		private IDictionary m_Data = null;
		private bool m_GenerateNextID = true;
		private string m_OperationStatus = null;
		private string m_Action = null;
		//Transaction Type holds the synchronization type or normal type
		private string m_Transaction_Type = "";
		
		public DataTransferObject() : base() {
		}

		public DataTransferObject(string loginID, string loginPassword, string language) {
			m_LoginID = loginID;
			m_LoginPassword = loginPassword;
			this.Language = language;
		}

		public DataTransferObject(string loginID, string loginPassword, int loginMemberID, string language) {
			m_LoginID = loginID;
			m_LoginPassword = loginPassword;
			this.Language = language;
			this.LoginMemberID = loginMemberID;
		}

		public DataTransferObject(IDictionary data) {
			m_Data = data;
		}
        [ProtoMember(1)]
		public string LoginID {
			get {
				return m_LoginID;
			}
			set {
				m_LoginID = value;
			}
		}
        [ProtoMember(2)]
		public string LoginPassword {
			get {
				return m_LoginPassword;
			}
			set {
				m_LoginPassword = value;
			}
		}
        [ProtoMember(3)]
		public int LoginMemberID {
			get {
				return m_LoginMemberID;
			}
			set {
				m_LoginMemberID = value;
			}
		}
        [ProtoMember(4)]
		public string Language {
			get {
				if (Strings.IsEmpty(m_Language)) {
					m_Language = Constants.LANGUAGE_DEFAULT;
				}
				return m_Language;
			}
			set {
			    if (Strings.IsEmpty(value)) {
			        m_Language = Constants.LANGUAGE_DEFAULT;
			    } else {
    				m_Language = value;
			    }
			}
		}
        [ProtoMember(5)]
		public bool GenerateNextID {
			get {
				return m_GenerateNextID;
			}
			set {
				m_GenerateNextID = value;
			}
		}
        [ProtoMember(6)]
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
        [ProtoMember(7)]
		public string OperationStatus {
			get {
				return m_OperationStatus;
			}
			set {
				m_OperationStatus = value;
			}
		}

        [ProtoMember(8)]
		public PagingDO Paging {
			get {
				if (Data[PAGING] == null) {
					Data[PAGING] = new PagingDO();
				}
				return (PagingDO)Data[PAGING];					
			}
			set {
				if (!Data.Contains(PAGING)) {
					Data.Add(PAGING, value);
				} else {
					Data[PAGING] = value;
				}
			}
		}
        [ProtoMember(9)]
		public string Transaction_Type {
			get {
				if (m_Transaction_Type.Equals("")) {
					m_Transaction_Type = Constants.TRANSACTION_TYPE_NORMAL; 	
				} 
				return m_Transaction_Type;
			}
			set {
				m_Transaction_Type = value;
			}
		}

		/// <summary>
		///     Convenience method to help determine whether the model is empty.
		/// </summary>
		/// 
		/// <returns>
		///     A bool value...
		/// </returns>
		public bool IsEmpty() {
			bool empty = false;

			if (this.GetType().UnderlyingSystemType.Equals(typeof (DataTransferObject))) {
				empty = true;
			}
			return empty;
		}

		public DateTime GetDefaultValue(DateTime dataObject) {
			if (((DateTime) dataObject).ToShortDateString().Equals("1/1/0001")) {
				dataObject = new DateTime(1900, 1, 1);
			}
			return dataObject;
		}

		public void GetSecurityData(SecurityDO security) {
			this.LoginID = security.LoginID;
			this.LoginPassword = security.LoginPassword;
//			this.LoginMemberID = security.LoginMemberID;
		}

		public void GetSecurityData(string loginID, string loginPassword, string language) {
			this.LoginID = loginID;
			this.LoginPassword = loginPassword;
			this.Language = language;
		}

		public void GetSecurityData(string loginID, string loginPassword, int loginMemberID, string language) {
			this.LoginID = loginID;
			this.LoginPassword = loginPassword;
			this.LoginMemberID = loginMemberID;
			this.Language = language;
		}

		public void GetSecurityData(DataTransferObject dataTransferObject) {
			this.LoginID = dataTransferObject.LoginID;
			this.LoginPassword = dataTransferObject.LoginPassword;
		}

		public string GetDefaultValue(string dataObject) {
			if (dataObject == null) {
				dataObject = "";
			}
			return dataObject;
		}

		public virtual object Clone() {
			return MemberwiseClone();
		}
	}
}