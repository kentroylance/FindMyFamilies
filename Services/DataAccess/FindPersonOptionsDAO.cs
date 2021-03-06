///////////////////////////////////////////////////////////////////////////
// Description: Data Access class for the 'Find_Person_Options'
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
	/// Purpose: Data Access class for the 'Find_Person_Options'.
	/// </summary>
	public class FindPersonOptionsDAO : FindPersonOptionsDAOBase {
		private static readonly object logCategory = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;
		// Stored Procedure Name Constants

		// Field Name Constants

		/// <summary>
		/// Purpose: Base Class constructor.
		/// </summary>
		public FindPersonOptionsDAO() : base() {
		}

		/// <summary>
		/// Purpose: Creates a new record in Find_Person_Options table and returns the generated primary key by reference.
		/// </summary>
		/// <param name = "findPersonOptionsDO">FindPersonOptionsDO findPersonOptionsDO</param>
		public override FindPersonOptionsDO CreateFindPersonOptions(FindPersonOptionsDO findPersonOptionsDO) {
			return base.CreateFindPersonOptions(findPersonOptionsDO);
		}

		/// <summary>
		/// Purpose: Update a record in Find_Person_Options.
		/// </summary>
		/// <param name = "findPersonOptionsDO">FindPersonOptionsDO findPersonOptionsDO</param>
		public override void UpdateFindPersonOptions(FindPersonOptionsDO findPersonOptionsDO) {
			base.UpdateFindPersonOptions(findPersonOptionsDO);
		}

		/// <summary>
		/// Purpose: Delete a record in Find_Person_Options.
		/// </summary>
		/// <param name = "findPersonOptionsDO">FindPersonOptionsDO findPersonOptions</param>
		public override void DeleteFindPersonOptions(FindPersonOptionsDO findPersonOptionsDO) {
			base.DeleteFindPersonOptions(findPersonOptionsDO);
		}

		/// <summary>
		/// Purpose: Retrieve a record in Find_Person_Options table by primary key.
		/// </summary>
		/// <param name = "findPersonOptionsDO">FindPersonOptionsDO findPersonOptionsDO.</param>
		public override FindPersonOptionsDO ReadFindPersonOptions(FindPersonOptionsDO findPersonOptionsDO) {
			return base.ReadFindPersonOptions(findPersonOptionsDO);
		}

		/// <summary>
		/// Purpose: Retrieves all records in Find_Person_Options table.
		/// </summary>
		/// <param name = "dataTransferObject">DataTransferObject dataTransferObject.</param>
		public override ICollection ReadFindPersonOptionssAll(DataTransferObject dataTransferObject) {
			return base.ReadFindPersonOptionssAll(dataTransferObject);
		}

		/// <summary>
		/// Purpose: Reads a page of records from table Find_Person_Options by search criteria.
		/// </summary>
		/// <returns>Returns a collection of FindPersonOptionss</returns>
		public virtual ICollection ReadFindPersonOptionssByPage(FindPersonOptionsDO findPersonOptionsDO) {
			IList findPersonOptionss = new ArrayList();
			IDataReader reader = null;
			try {
				IDataCommand command = dataSource.GetCommand(PagingDO.READ_BY_PAGE);
				command.SetParameter(PagingDO.PAGING_TABLE_NAME, TABLE_NAME);
				command.SetParameter(PagingDO.PAGING_PRIMARY_KEY, FindPersonOptionsDAO.FIND_PERSON_OPTIONS_ID);
				command.SetParameter(PagingDO.PAGING_FIELDS, "*");
				command.SetParameter(PagingDO.PAGING_ORDER, FindPersonOptionsDAO.FIND_PERSON_OPTIONS_ID);
				command.SetParameter(PagingDO.PAGING_PAGE_SIZE, findPersonOptionsDO.Paging.PageSize);
				command.SetParameter(PagingDO.PAGING_PAGE_INDEX, findPersonOptionsDO.Paging.PageIndex);
				command.SetParameter(PagingDO.PAGING_FILTER, FindPersonOptionsDAO.FIND_PERSON_OPTIONS_ID + " > 1");
				command.SetParameter(LANGUAGE, findPersonOptionsDO.Language);
				reader = command.ExecuteReader();
		
				// populate the detail object from the SQL Server Data Reader
				while (reader.Read()) {
					findPersonOptionss.Add(this.MapToDataTransferObject(reader));
				}
				if (reader.NextResult()) {
					reader.Read();
					if (findPersonOptionss.Count < 1) {
						findPersonOptionss.Add(new FindPersonOptionsDO());
					}
					((FindPersonOptionsDO)findPersonOptionss[0]).Paging.TotalRecords = reader.GetInt32(0);
				}
				ProcessResult(reader, findPersonOptionsDO, logCategory);
			} catch (Exception ex) {
				string errorMessage = Resource.GetErrorMessage(MessageKeys.FIND_PERSON_OPTIONS_CANNOT_READ_BY_PAGE, findPersonOptionsDO.Language, ex);
				throw new DataAccessException(errorMessage, ex, logCategory);
			} finally {
				if (reader != null) {
					reader.Close();
				}
			}
			return findPersonOptionss;
		}

	}
}

