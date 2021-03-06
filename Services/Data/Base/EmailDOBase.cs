///////////////////////////////////////////////////////////////////////////
// Description: Data Object Base for the 'Email'
// Generated by FindMyFamilies Generator
///////////////////////////////////////////////////////////////////////////
using System;

using FindMyFamilies.Util;
using FindMyFamilies.Data;
using ProtoBuf;

// Please do not modify any code in this auto generated class.  This code will be overwritten when the generator is executed.
namespace FindMyFamilies.Data {

	/// <summary>
	/// Purpose: DataTransferObject Base class for the 'Email'.
	/// </summary>
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields, AsReferenceDefault = true)]
	public class EmailDOBase : DataTransferObject {

		// Email Constants
		// Email Constants
		public const string TABLE_NAME = "EMAIL";
		public const string EMAIL_ID = "EmailID";
		public const string EMAIL = "Email";
		public const string DATE_CREATED = "DateCreated";
		public const string LAST_EMAIL_SENT = "LastEmailSent";
		public const string PERSON_ID = "PersonID";
		public const string LANGUAGE_ID = "LanguageID";
		public const string ALLOWED_TO_SEND = "AllowedToSend";

		// Property Declarations
		private int		m_EmailID;  // Email_ID
		private string	m_Email;  // Email
		private DateTime	m_DateCreated;  // Date_Created
		private DateTime	m_LastEmailSent;  // Last_Email_Sent
		private string	m_PersonID;  // Person_ID
		private string	m_LanguageID;  // Language_ID
		private bool		m_AllowedToSend;  // Allowed_To_Send

		/// <summary>
		/// Purpose: Class constructor.
		/// </summary>
		public EmailDOBase() {
			this.LanguageID = Constants.LANGUAGE_DEFAULT;
		}


		// Class Property Declarations
		/// <summary>
		/// SQLFieldName: Email_ID
		/// </summary>
		public int EmailID {
			get {
				return m_EmailID;
			}
			set {
				m_EmailID = value;
			}
		}

		/// <summary>
		/// SQLFieldName: Email
		/// </summary>
		public string Email {
			get {
				return GetDefaultValue(m_Email);
			}
			set {
				m_Email = value;
			}
		}

		/// <summary>
		/// SQLFieldName: Date_Created
		/// </summary>
		public DateTime DateCreated {
			get {
				return GetDefaultValue(m_DateCreated);
			}
			set {
				m_DateCreated = value;
			}
		}

		/// <summary>
		/// SQLFieldName: Last_Email_Sent
		/// </summary>
		public DateTime LastEmailSent {
			get {
				return GetDefaultValue(m_LastEmailSent);
			}
			set {
				m_LastEmailSent = value;
			}
		}

		/// <summary>
		/// SQLFieldName: Person_ID
		/// </summary>
		public string PersonID {
			get {
				return GetDefaultValue(m_PersonID);
			}
			set {
				m_PersonID = value;
			}
		}

		/// <summary>
		/// SQLFieldName: Language_ID
		/// </summary>
		public string LanguageID {
			get {
				return GetDefaultValue(m_LanguageID);
			}
			set {
				m_LanguageID = value;
			}
		}

		/// <summary>
		/// SQLFieldName: Allowed_To_Send
		/// </summary>
		public bool AllowedToSend {
			get {
				return m_AllowedToSend;
			}
			set {
				m_AllowedToSend = value;
			}
		}

	}
}
