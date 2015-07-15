///////////////////////////////////////////////////////////////////////////
// Description: Data Access class for the 'Email'
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

namespace FindMyFamilies.DataAccess {
	/// <summary>
	/// Purpose: Data Access class for the 'Email'.
	/// </summary>
	public class EmailDAO : EmailDAOBase {
		private static readonly object logCategory = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;
		// Stored Procedure Name Constants

		// Field Name Constants
	    public const string READ_BY_EMAIL = "ReadEmailByEmail";

		/// <summary>
		/// Purpose: Base Class constructor.
		/// </summary>
		public EmailDAO() : base() {
		}

		/// <summary>
		/// Purpose: Creates a new record in Email table and returns the generated primary key by reference.
		/// </summary>
		/// <param name = "emailDO">EmailDO emailDO</param>
		public override EmailDO CreateEmail(EmailDO emailDO) {
			return base.CreateEmail(emailDO);
		}

		/// <summary>
		/// Purpose: Update a record in Email.
		/// </summary>
		/// <param name = "emailDO">EmailDO emailDO</param>
		public override void UpdateEmail(EmailDO emailDO) {
			base.UpdateEmail(emailDO);
		}

		/// <summary>
		/// Purpose: Delete a record in Email.
		/// </summary>
		/// <param name = "emailDO">EmailDO email</param>
		public override void DeleteEmail(EmailDO emailDO) {
			base.DeleteEmail(emailDO);
		}

		/// <summary>
		/// Purpose: RetrieveFamilySearchData a record in Email table by primary key.
		/// </summary>
		/// <param name = "emailDO">EmailDO emailDO.</param>
		public override EmailDO ReadEmail(EmailDO emailDO) {
			return base.ReadEmail(emailDO);
		}

		/// <summary>
		/// Purpose: Retrieves all records in Email table.
		/// </summary>
		/// <param name = "dataTransferObject">DataTransferObject dataTransferObject.</param>
		public override ICollection ReadEmailsAll(DataTransferObject dataTransferObject) {
			return base.ReadEmailsAll(dataTransferObject);
		}

		/// <summary>
		/// Purpose: Retrieves all records from Email table for populating a list control.
		/// </summary>
		/// <param name = "dataTransferObject">DataTransferObject dataTransferObject</param>
		public override ICollection ReadEmailsAllList(DataTransferObject dataTransferObject) {
			return base.ReadEmailsAllList(dataTransferObject);
		}

		/// <summary>
		/// Purpose: Reads a page of records from table Email by search criteria.
		/// </summary>
		/// <returns>Returns a collection of Emails</returns>
		public virtual ICollection ReadEmailsByPage(EmailDO emailDO) {
			IList emails = new ArrayList();
			IDataReader reader = null;
			try {
				IDataCommand command = dataSource.GetCommand(PagingDO.READ_BY_PAGE);
				command.SetParameter(PagingDO.PAGING_TABLE_NAME, TABLE_NAME);
				command.SetParameter(PagingDO.PAGING_PRIMARY_KEY, EmailDAO.EMAIL_ID);
				command.SetParameter(PagingDO.PAGING_FIELDS, "*");
				command.SetParameter(PagingDO.PAGING_ORDER, EmailDAO.EMAIL_ID);
				command.SetParameter(PagingDO.PAGING_PAGE_SIZE, emailDO.Paging.PageSize);
				command.SetParameter(PagingDO.PAGING_PAGE_INDEX, emailDO.Paging.PageIndex);
				command.SetParameter(PagingDO.PAGING_FILTER, EmailDAO.EMAIL_ID + " > 1");
				command.SetParameter(LANGUAGE, emailDO.Language);
				reader = command.ExecuteReader();
		
				// populate the detail object from the SQL Server Data Reader
				while (reader.Read()) {
					emails.Add(this.MapToDataTransferObject(reader));
				}
				if (reader.NextResult()) {
					reader.Read();
					if (emails.Count < 1) {
						emails.Add(new EmailDO());
					}
					((EmailDO)emails[0]).Paging.TotalRecords = reader.GetInt32(0);
				}
				ProcessResult(reader, emailDO, logCategory);
			} catch (Exception ex) {
				string errorMessage = Resource.GetErrorMessage(MessageKeys.EMAIL_CANNOT_READ_BY_PAGE, emailDO.Language, ex);
				throw new DataAccessException(errorMessage, ex, logCategory);
			} finally {
				if (reader != null) {
					reader.Close();
				}
			}
			return emails;
		}

        /// <summary>
		/// Purpose: Reads any email that was subscribed to that matches this email
		/// </summary>
		/// <returns>Returns a email object</returns>
		public virtual EmailDO ReadEmailByEmail(string email) {
			IDataReader reader = null;
    		var emailDO = new EmailDO();
			try {
				IDataCommand command = dataSource.GetCommand(READ_BY_EMAIL);
				command.SetParameter(EmailDAO.EMAIL, email);
				command.SetParameter(LANGUAGE, Constants.LANGUAGE_DEFAULT);
				reader = command.ExecuteReader();

				// populate the detail object from the SQL Server Data Reader
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
				string errorMessage = Resource.GetErrorMessage(MessageKeys.EMAIL_CANNOT_READ_BY_EMAIL, emailDO.Language, ex);
				throw new DataAccessException(errorMessage, ex, logCategory);
			} finally {
				if (reader != null) {
					reader.Close();
				}
			}
			return emailDO;
		}


	}


}
