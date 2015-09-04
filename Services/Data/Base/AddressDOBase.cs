///////////////////////////////////////////////////////////////////////////
// Description: Data Object Base for the 'Address'
// Generated by FindMyFamilies Generator
///////////////////////////////////////////////////////////////////////////
using System;

using FindMyFamilies.Util;
using FindMyFamilies.Data;

// Please do not modify any code in this auto generated class.  This code will be overwritten when the generator is executed.
namespace FindMyFamilies.Data {

	/// <summary>
	/// Purpose: DataTransferObject Base class for the 'Address'.
	/// </summary>
	public class AddressDOBase : DataTransferObject {

		// Address Constants
		// Address Constants
		public const string TABLE_NAME = "ADDRESS";
		public const string ADDRESS_ID = "AddressID";
		public const string ASSOCIATED_ID = "AssociatedID";
		public const string ADDRESS_TYPE = "AddressType";
		public const string ADDRESS_STATUS = "AddressStatus";
		public const string ADDRESS_LINE1 = "AddressLine1";
		public const string ADDRESS_LINE2 = "AddressLine2";
		public const string ADDRESS_LINE3 = "AddressLine3";
		public const string CITY = "City";
		public const string STATE = "State";
		public const string ZIP_CODE = "ZipCode";
		public const string COUNTRY = "Country";
		public const string DAY_PHONE = "DayPhone";
		public const string NIGHT_PHONE = "NightPhone";
		public const string CELL_PHONE = "CellPhone";
		public const string FAX_NUMBER = "FaxNumber";
		public const string REGION = "Region";
		public const string LANGUAGE_ID = "LanguageID";

		// Property Declarations
		private int		m_AddressID;  // Address_ID
		private int		m_AssociatedID;  // Associated_ID
		private string	m_AddressType;  // Address_Type
		private string	m_AddressStatus;  // Address_Status
		private string	m_AddressLine1;  // Address_Line1
		private string	m_AddressLine2;  // Address_Line2
		private string	m_AddressLine3;  // Address_Line3
		private string	m_City;  // City
		private string	m_State;  // State
		private string	m_ZipCode;  // Zip_Code
		private string	m_Country;  // Country
		private string	m_DayPhone;  // Day_Phone
		private string	m_NightPhone;  // Night_Phone
		private string	m_CellPhone;  // Cell_Phone
		private string	m_FaxNumber;  // Fax_Number
		private string	m_Region;  // Region
		private string	m_LanguageID;  // Language_ID

		/// <summary>
		/// Purpose: Class constructor.
		/// </summary>
		public AddressDOBase() {
		}


		// Class Property Declarations
		/// <summary>
		/// SQLFieldName: Address_ID
		/// </summary>
		public int AddressID {
			get {
				return m_AddressID;
			}
			set {
				m_AddressID = value;
			}
		}

		/// <summary>
		/// SQLFieldName: Associated_ID
		/// </summary>
		public int AssociatedID {
			get {
				return m_AssociatedID;
			}
			set {
				m_AssociatedID = value;
			}
		}

		/// <summary>
		/// SQLFieldName: Address_Type
		/// </summary>
		public string AddressType {
			get {
				return GetDefaultValue(m_AddressType);
			}
			set {
				m_AddressType = value;
			}
		}

		/// <summary>
		/// SQLFieldName: Address_Status
		/// </summary>
		public string AddressStatus {
			get {
				return GetDefaultValue(m_AddressStatus);
			}
			set {
				m_AddressStatus = value;
			}
		}

		/// <summary>
		/// SQLFieldName: Address_Line1
		/// </summary>
		public string AddressLine1 {
			get {
				return GetDefaultValue(m_AddressLine1);
			}
			set {
				m_AddressLine1 = value;
			}
		}

		/// <summary>
		/// SQLFieldName: Address_Line2
		/// </summary>
		public string AddressLine2 {
			get {
				return GetDefaultValue(m_AddressLine2);
			}
			set {
				m_AddressLine2 = value;
			}
		}

		/// <summary>
		/// SQLFieldName: Address_Line3
		/// </summary>
		public string AddressLine3 {
			get {
				return GetDefaultValue(m_AddressLine3);
			}
			set {
				m_AddressLine3 = value;
			}
		}

		/// <summary>
		/// SQLFieldName: City
		/// </summary>
		public string City {
			get {
				return GetDefaultValue(m_City);
			}
			set {
				m_City = value;
			}
		}

		/// <summary>
		/// SQLFieldName: State
		/// </summary>
		public string State {
			get {
				return GetDefaultValue(m_State);
			}
			set {
				m_State = value;
			}
		}

		/// <summary>
		/// SQLFieldName: Zip_Code
		/// </summary>
		public string ZipCode {
			get {
				return GetDefaultValue(m_ZipCode);
			}
			set {
				m_ZipCode = value;
			}
		}

		/// <summary>
		/// SQLFieldName: Country
		/// </summary>
		public string Country {
			get {
				return GetDefaultValue(m_Country);
			}
			set {
				m_Country = value;
			}
		}

		/// <summary>
		/// SQLFieldName: Day_Phone
		/// </summary>
		public string DayPhone {
			get {
				return GetDefaultValue(m_DayPhone);
			}
			set {
				m_DayPhone = value;
			}
		}

		/// <summary>
		/// SQLFieldName: Night_Phone
		/// </summary>
		public string NightPhone {
			get {
				return GetDefaultValue(m_NightPhone);
			}
			set {
				m_NightPhone = value;
			}
		}

		/// <summary>
		/// SQLFieldName: Cell_Phone
		/// </summary>
		public string CellPhone {
			get {
				return GetDefaultValue(m_CellPhone);
			}
			set {
				m_CellPhone = value;
			}
		}

		/// <summary>
		/// SQLFieldName: Fax_Number
		/// </summary>
		public string FaxNumber {
			get {
				return GetDefaultValue(m_FaxNumber);
			}
			set {
				m_FaxNumber = value;
			}
		}

		/// <summary>
		/// SQLFieldName: Region
		/// </summary>
		public string Region {
			get {
				return GetDefaultValue(m_Region);
			}
			set {
				m_Region = value;
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

	}
}
