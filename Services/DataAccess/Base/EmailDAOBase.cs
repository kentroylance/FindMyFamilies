///////////////////////////////////////////////////////////////////////////
// Description: Data Access Base class for the 'Email'
// Generated by FindMyFamilies Generator
///////////////////////////////////////////////////////////////////////////
using System;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.EnterpriseServices;
using System.Reflection;

using FindMyFamilies.Data;
using FindMyFamilies.Util;
using FindMyFamilies.Exceptions;
using FindMyFamilies.DataAccess;

// Please do not modify any code in this auto generated class.  This code will be overwritten when the generator is executed.
namespace FindMyFamilies.DataAccess {

	/// <summary>
	/// Purpose: Data Access Base class for table 'Email'.
	/// </summary>
	public class EmailDAOBase : DataAccessObjectBase {
		private static readonly object logCategory = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;

		// Stored Procedure Constants
		public const string TABLE_NAME = "EMAIL";
		public const string TABLE_NAME_KEY = "Email_SEQ";

		public const string CREATE = "CreateEmail";
		public const string UPDATE = "UpdateEmail";
		public const string DELETE = "DeleteEmail";
		public const string DELETE_ALL = "DeleteAllEmails";
		public const string READ = "ReadEmail";
		public const string READ_ALL = "ReadAllEmails";

		// Field Name Constants
		public const string EMAIL_ID = "email_id";
		public const string EMAIL = "email";
		public const string DATE_CREATED = "date_created";
		public const string LAST_EMAIL_SENT = "last_email_sent";
		public const string PERSON_ID = "person_id";
		public const string LANGUAGE_ID = "language_id";
		public const string ALLOWED_TO_SEND = "allowed_to_send";

		/// <summary>
		/// Purpose: Class constructor.
		/// </summary>
		public EmailDAOBase() : base() {
		}

		/// <summary>
		/// Purpose: Maps the fields of the resultsset to the attributes of the Email data object
		/// </summary>
		/// <returns>Email data object populated with data from the result set</returns>
		/// <param name = "reader">IDataReader reader</param>
		public virtual EmailDO MapToDataTransferObject(IDataReader reader) {
			EmailDO emailDO = new EmailDO();
			emailDO.EmailID = (int)GetValue(reader, EMAIL_ID);
			emailDO.Email = (string)GetValue(reader, EMAIL);
			emailDO.DateCreated = (DateTime)GetValue(reader, DATE_CREATED);
			emailDO.LastEmailSent = (DateTime)GetValue(reader, LAST_EMAIL_SENT);
			emailDO.PersonID = (string)GetValue(reader, PERSON_ID);
			emailDO.LanguageID = (string)GetValue(reader, LANGUAGE_ID);
			emailDO.AllowedToSend = (bool)GetValue(reader, ALLOWED_TO_SEND);
			emailDO.GenerateNextID = false;
			return emailDO;
		}

		/// <summary>
		/// Purpose: Creates a record in Email table and returns the generated primary key by reference.
		/// </summary>
		/// <param name = "emailDO">EmailDO emailDO</param>
		public virtual EmailDO CreateEmail(EmailDO emailDO) {
			IDataReader reader = null;
			try {
				IDataCommand command = dataSource.GetCommand(CREATE);
				command.SetParameter(EmailDAO.EMAIL, emailDO.Email);
				command.SetParameter(EmailDAO.DATE_CREATED, emailDO.DateCreated);
				command.SetParameter(EmailDAO.LAST_EMAIL_SENT, emailDO.LastEmailSent);
				command.SetParameter(EmailDAO.PERSON_ID, emailDO.PersonID);
				command.SetParameter(EmailDAO.LANGUAGE, emailDO.Language);
				command.SetParameter(EmailDAO.ALLOWED_TO_SEND, emailDO.AllowedToSend);
				reader = command.ExecuteReader();
				if (reader.Read()) {
					emailDO.EmailID = reader.GetInt32(0);
				}
				ProcessResult(reader, emailDO.EmailID, emailDO, Constants.OPERATION_CREATE, logCategory);
			} catch (Exception ex) {
				string errorMessage = Resource.GetErrorMessage(MessageKeys.EMAIL_CANNOT_CREATE, emailDO.Language, ex);
				throw new DataAccessException(errorMessage, ex, logCategory);
			} finally {
				if (reader != null) {
					reader.Close();
				}
			}
			return emailDO;
		}


		/// <summary>
		/// Purpose: Updates and existing record in Email table.
		/// </summary>
		/// <param name = "emailDO">EmailDO emailDO.</param>
		public virtual void UpdateEmail(EmailDO emailDO) {
			IDataReader reader = null;
			try {
				IDataCommand command = dataSource.GetCommand(UPDATE);
				command.SetParameter(EmailDAO.EMAIL_ID, emailDO.EmailID);
				command.SetParameter(EmailDAO.EMAIL, emailDO.Email);
				command.SetParameter(EmailDAO.DATE_CREATED, emailDO.DateCreated);
				command.SetParameter(EmailDAO.LAST_EMAIL_SENT, emailDO.LastEmailSent);
				command.SetParameter(EmailDAO.PERSON_ID, emailDO.PersonID);
				command.SetParameter(EmailDAO.LANGUAGE, emailDO.Language);
				command.SetParameter(EmailDAO.ALLOWED_TO_SEND, emailDO.AllowedToSend);
				reader = command.ExecuteReader();
				ProcessResult(reader, emailDO, logCategory);
			} catch (Exception ex) {
				string errorMessage = Resource.GetErrorMessage(MessageKeys.EMAIL_CANNOT_UPDATE, emailDO.Language, ex);
				throw new DataAccessException(errorMessage, ex, logCategory);
			} finally {
				if (reader != null) {
					reader.Close();
				}
			}
		}

		/// <summary>
		/// Purpose: Delete record in Email table
		/// </summary>
		/// <param name = "emailDO">EmailDO emailDO.</param>
		public virtual void DeleteEmail(EmailDO emailDO) {
			IDataReader reader = null;
			try {
				IDataCommand command = dataSource.GetCommand(DELETE);
				command.SetParameter(EmailDAO.EMAIL_ID, emailDO.EmailID);
				reader = command.ExecuteReader();
				ProcessResult(reader, emailDO, logCategory);
			} catch (Exception ex) {
				string errorMessage = Resource.GetErrorMessage(MessageKeys.EMAIL_CANNOT_DELETE, emailDO.Language, ex);
				throw new DataAccessException(errorMessage, ex, logCategory);
			} finally {
				if (reader != null) {
					reader.Close();
				}
			}
		}

		/// <summary>
		/// Purpose: Reads a record by primary key in Email table.
		/// </summary>
		/// <param name = "emailDO">EmailDO emailDO.</param>
		public virtual EmailDO ReadEmail(EmailDO emailDO) {
			IDataReader reader = null;
			try {
				IDataCommand command = dataSource.GetCommand(READ);
				command.SetParameter(EmailDAO.EMAIL_ID, emailDO.EmailID);
				reader = command.ExecuteReader();

				// populate the detail object from the SQL Server Data Reader
				emailDO = new EmailDO();
				if (reader.Read()) {
					emailDO.EmailID = (int)GetValue(reader, EMAIL_ID);
					emailDO.Email = (string)GetValue(reader, EMAIL);
					emailDO.DateCreated = (DateTime)GetValue(reader, DATE_CREATED);
					emailDO.LastEmailSent = (DateTime)GetValue(reader, LAST_EMAIL_SENT);
					emailDO.PersonID = (string)GetValue(reader, PERSON_ID);
					emailDO.LanguageID = (string)GetValue(reader, LANGUAGE_ID);
					emailDO.AllowedToSend = (bool)GetValue(reader, ALLOWED_TO_SEND);
				}
				ProcessResult(reader, emailDO, logCategory);
			} catch (Exception ex) {
				string errorMessage = Resource.GetErrorMessage(MessageKeys.EMAIL_CANNOT_READ, emailDO.Language, ex);
				throw new DataAccessException(errorMessage, ex, logCategory);
			} finally {
				if (reader != null) {
					reader.Close();
				}
			}
			return emailDO;
		}

		/// <summary>
		/// Purpose: Reads all the records from the Email table.
		/// </summary>
		/// <param name = "dataTransferObject">DataTransferObject dataTransferObject</param>
		public virtual ICollection ReadEmailsAll(DataTransferObject dataTransferObject) {
			IList emails = new ArrayList();
			IDataReader reader = null;
			try {
				IDataCommand command = dataSource.GetCommand(READ_ALL);
				command.SetParameter(LANGUAGE, dataTransferObject.Language);
				reader = command.ExecuteReader();

				// populate the detail object from the SQL Server Data Reader
				while (reader.Read()) {
					emails.Add(this.MapToDataTransferObject(reader));
				}
				ProcessResult(reader, dataTransferObject, logCategory);
			} catch (Exception ex) {
				string errorMessage = Resource.GetErrorMessage(MessageKeys.EMAIL_CANNOT_READ, dataTransferObject.Language, ex);
				throw new DataAccessException(errorMessage, ex, logCategory);
			} finally {
				if (reader != null) {
					reader.Close();
				}
			}
			return emails;
		}

		/// <summary>
		/// Purpose: Reads all the records from the Email table to populate list control.
		/// </summary>
		/// <param name = "dataTransferObject">DataTransferObject dataTransferObject</param>
		/// <returns>Returns a collection of ListItem Data Objects</returns>
		public virtual ICollection ReadEmailsAllList(DataTransferObject dataTransferObject) {
			IList emails = new ArrayList();
			IDataReader reader = null;
			try {
				IDataCommand command = dataSource.GetCommand(MethodBase.GetCurrentMethod().Name);
				command.SetParameter(LANGUAGE, dataTransferObject.Language);
				reader = command.ExecuteReader();

				// populate the detail object from the SQL Server Data Reader
				ListItemDO listItem = null;
				while (reader.Read()) {
					listItem = new ListItemDO();
					listItem.ValueMember = Convert.ToString(GetValue(reader, EmailDO.EMAIL_ID));
					listItem.DisplayMember = (string)GetValue(reader, EmailDO.DATE_CREATED);
					emails.Add(listItem);
				}
				ProcessResult(reader, dataTransferObject, logCategory);
			} catch (Exception ex) {
				string errorMessage = Resource.GetErrorMessage(MessageKeys.EMAIL_CANNOT_READ_ALL, dataTransferObject.Language, ex);
				throw new DataAccessException(errorMessage, ex, logCategory);
			} finally {
				if (reader != null) {
					reader.Close();
				}
			}
			return emails;
		}

	}
}
