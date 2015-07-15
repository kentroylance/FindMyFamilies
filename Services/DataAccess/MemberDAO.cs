///////////////////////////////////////////////////////////////////////////
// Description: Data Access class for the 'Member'
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
	/// Purpose: Data Access class for the 'Member'.
	/// </summary>
	public class MemberDAO : MemberDAOBase {
		private static readonly object logCategory = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;
		// Stored Procedure Name Constants
        public const string READ_BY_MEMBER_ID = "ReadMemberByMemberID";
        public const string READ_BY_PERSON_ID = "ReadMemberByPersonID";

		// Field Name Constants

		/// <summary>
		/// Purpose: Base Class constructor.
		/// </summary>
		public MemberDAO() : base() {
		}

		/// <summary>
		/// Purpose: Creates a new record in Member table and returns the generated primary key by reference.
		/// </summary>
		/// <param name = "memberDO">MemberDO memberDO</param>
		public override MemberDO CreateMember(MemberDO memberDO) {
			return base.CreateMember(memberDO);
		}

		/// <summary>
		/// Purpose: Update a record in Member.
		/// </summary>
		/// <param name = "memberDO">MemberDO memberDO</param>
		public override void UpdateMember(MemberDO memberDO) {
			base.UpdateMember(memberDO);
		}

		/// <summary>
		/// Purpose: Delete a record in Member.
		/// </summary>
		/// <param name = "memberDO">MemberDO member</param>
		public override void DeleteMember(MemberDO memberDO) {
			base.DeleteMember(memberDO);
		}

		/// <summary>
		/// Purpose: RetrieveFamilySearchData a record in Member table by primary key.
		/// </summary>
		/// <param name = "memberDO">MemberDO memberDO.</param>
		public override MemberDO ReadMember(MemberDO memberDO) {
			return base.ReadMember(memberDO);
		}

		/// <summary>
		/// Purpose: Retrieves all records in Member table.
		/// </summary>
		/// <param name = "dataTransferObject">DataTransferObject dataTransferObject.</param>
		public override ICollection ReadMembersAll(DataTransferObject dataTransferObject) {
			return base.ReadMembersAll(dataTransferObject);
		}

		/// <summary>
		/// Purpose: Reads a page of records from table Member by search criteria.
		/// </summary>
		/// <returns>Returns a collection of Members</returns>
		public virtual ICollection ReadMembersByPage(MemberDO memberDO) {
			IList members = new ArrayList();
			IDataReader reader = null;
			try {
				IDataCommand command = dataSource.GetCommand(PagingDO.READ_BY_PAGE);
				command.SetParameter(PagingDO.PAGING_TABLE_NAME, TABLE_NAME);
				command.SetParameter(PagingDO.PAGING_PRIMARY_KEY, MemberDAO.MEMBER_ID);
				command.SetParameter(PagingDO.PAGING_FIELDS, "*");
				command.SetParameter(PagingDO.PAGING_ORDER, MemberDAO.MEMBER_ID);
				command.SetParameter(PagingDO.PAGING_PAGE_SIZE, memberDO.Paging.PageSize);
				command.SetParameter(PagingDO.PAGING_PAGE_INDEX, memberDO.Paging.PageIndex);
				command.SetParameter(PagingDO.PAGING_FILTER, MemberDAO.MEMBER_ID + " > 1");
				command.SetParameter(LANGUAGE, memberDO.Language);
				reader = command.ExecuteReader();
		
				// populate the detail object from the SQL Server Data Reader
				while (reader.Read()) {
					members.Add(this.MapToDataTransferObject(reader));
				}
				if (reader.NextResult()) {
					reader.Read();
					if (members.Count < 1) {
						members.Add(new MemberDO());
					}
					((MemberDO)members[0]).Paging.TotalRecords = reader.GetInt32(0);
				}
				ProcessResult(reader, memberDO, logCategory);
			} catch (Exception ex) {
				string errorMessage = Resource.GetErrorMessage(MessageKeys.MEMBER_CANNOT_READ_BY_PAGE, memberDO.Language, ex);
				throw new DataAccessException(errorMessage, ex, logCategory);
			} finally {
				if (reader != null) {
					reader.Close();
				}
			}
			return members;
		}

        /// Purpose: Read a member by member id
		/// </summary>
		/// <returns>Returns a member object</returns>
		public virtual MemberDO ReadMemberByMemberID(int memberID) {
			IDataReader reader = null;
    		var memberDO = new MemberDO();
			try {
				IDataCommand command = dataSource.GetCommand(READ_BY_MEMBER_ID);
				command.SetParameter(MemberDAO.MEMBER_ID, memberID);
				command.SetParameter(LANGUAGE, Constants.LANGUAGE_DEFAULT);
				reader = command.ExecuteReader();

				// populate the detail object from the SQL Server Data Reader
				if (reader.Read()) {
			        memberDO.MemberID = (int)GetValue(reader, MEMBER_ID);
			        memberDO.PersonID = (string)GetValue(reader, PERSON_ID);
			        memberDO.MailingAddressID = (int)GetValue(reader, MAILING_ADDRESS_ID);
			        memberDO.MemberType = (string)GetValue(reader, MEMBER_TYPE);
			        memberDO.MemberStatus = (string)GetValue(reader, MEMBER_STATUS);
			        memberDO.Firstname = (string)GetValue(reader, FIRSTNAME);
			        memberDO.Middlename = (string)GetValue(reader, MIDDLENAME);
			        memberDO.Lastname = (string)GetValue(reader, LASTNAME);
			        memberDO.Email = (string)GetValue(reader, EMAIL);
			        memberDO.BirthDate = (DateTime)GetValue(reader, BIRTH_DATE);
			        memberDO.Password = (string)GetValue(reader, PASSWORD);
			        memberDO.DateCreated = (DateTime)GetValue(reader, DATE_CREATED);
			        memberDO.DisplayName = (string)GetValue(reader, DISPLAY_NAME);
			        memberDO.Notes = (string)GetValue(reader, NOTES);
				}
				ProcessResult(reader, memberDO, logCategory);
			} catch (Exception ex) {
				string errorMessage = Resource.GetErrorMessage(MessageKeys.MEMBER_CANNOT_READ_BY_MEMBER, memberDO.Language, ex);
				throw new DataAccessException(errorMessage, ex, logCategory);
			} finally {
				if (reader != null) {
					reader.Close();
				}
			}
			return memberDO;
		}

                /// Purpose: Read a member by person id
		/// </summary>
		/// <returns>Returns a member object</returns>
		public virtual MemberDO ReadMemberByPersonID(string personID) {
			IDataReader reader = null;
    		var memberDO = new MemberDO();
			try {
				IDataCommand command = dataSource.GetCommand(READ_BY_PERSON_ID);
				command.SetParameter(MemberDAO.PERSON_ID, personID);
				command.SetParameter(LANGUAGE, Constants.LANGUAGE_DEFAULT);
				reader = command.ExecuteReader();

				// populate the detail object from the SQL Server Data Reader
				if (reader.Read()) {
			        memberDO.MemberID = (int)GetValue(reader, MEMBER_ID);
			        memberDO.PersonID = (string)GetValue(reader, PERSON_ID);
			        memberDO.MailingAddressID = (int)GetValue(reader, MAILING_ADDRESS_ID);
			        memberDO.MemberType = (string)GetValue(reader, MEMBER_TYPE);
			        memberDO.MemberStatus = (string)GetValue(reader, MEMBER_STATUS);
			        memberDO.Firstname = (string)GetValue(reader, FIRSTNAME);
			        memberDO.Middlename = (string)GetValue(reader, MIDDLENAME);
			        memberDO.Lastname = (string)GetValue(reader, LASTNAME);
			        memberDO.Email = (string)GetValue(reader, EMAIL);
			        memberDO.BirthDate = (DateTime)GetValue(reader, BIRTH_DATE);
			        memberDO.Password = (string)GetValue(reader, PASSWORD);
			        memberDO.DateCreated = (DateTime)GetValue(reader, DATE_CREATED);
			        memberDO.DisplayName = (string)GetValue(reader, DISPLAY_NAME);
			        memberDO.Notes = (string)GetValue(reader, NOTES);
				}
				ProcessResult(reader, memberDO, logCategory);
			} catch (Exception ex) {
				string errorMessage = Resource.GetErrorMessage(MessageKeys.MEMBER_CANNOT_READ_BY_PERSON, memberDO.Language, ex);
				throw new DataAccessException(errorMessage, ex, logCategory);
			} finally {
				if (reader != null) {
					reader.Close();
				}
			}
			return memberDO;
		}


	}
}
