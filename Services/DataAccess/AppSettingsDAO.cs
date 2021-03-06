///////////////////////////////////////////////////////////////////////////
// Description: Data Access class for the 'App_Settings'
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
	/// Purpose: Data Access class for the 'App_Settings'.
	/// </summary>
	public class AppSettingsDAO : AppSettingsDAOBase {
		private static readonly object logCategory = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;
		// Stored Procedure Name Constants

		// Field Name Constants

		/// <summary>
		/// Purpose: Base Class constructor.
		/// </summary>
		public AppSettingsDAO() : base() {
		}

		/// <summary>
		/// Purpose: Creates a new record in App_Settings table and returns the generated primary key by reference.
		/// </summary>
		/// <param name = "appSettingsDO">AppSettingsDO appSettingsDO</param>
		public override AppSettingsDO CreateAppSettings(AppSettingsDO appSettingsDO) {
			return base.CreateAppSettings(appSettingsDO);
		}

		/// <summary>
		/// Purpose: Update a record in App_Settings.
		/// </summary>
		/// <param name = "appSettingsDO">AppSettingsDO appSettingsDO</param>
		public override void UpdateAppSettings(AppSettingsDO appSettingsDO) {
			base.UpdateAppSettings(appSettingsDO);
		}

		/// <summary>
		/// Purpose: Delete a record in App_Settings.
		/// </summary>
		/// <param name = "appSettingsDO">AppSettingsDO appSettings</param>
		public override void DeleteAppSettings(AppSettingsDO appSettingsDO) {
			base.DeleteAppSettings(appSettingsDO);
		}

		/// <summary>
		/// Purpose: Retrieve a record in App_Settings table by primary key.
		/// </summary>
		/// <param name = "appSettingsDO">AppSettingsDO appSettingsDO.</param>
		public override AppSettingsDO ReadAppSettings(AppSettingsDO appSettingsDO) {
			return base.ReadAppSettings(appSettingsDO);
		}

		/// <summary>
		/// Purpose: Retrieves all records in App_Settings table.
		/// </summary>
		/// <param name = "dataTransferObject">DataTransferObject dataTransferObject.</param>
		public override ICollection ReadAppSettingssAll(DataTransferObject dataTransferObject) {
			return base.ReadAppSettingssAll(dataTransferObject);
		}

		/// <summary>
		/// Purpose: Retrieves all records from App_Settings table for populating a list control.
		/// </summary>
		/// <param name = "dataTransferObject">DataTransferObject dataTransferObject</param>
		public override ICollection ReadAppSettingssAllList(DataTransferObject dataTransferObject) {
			return base.ReadAppSettingssAllList(dataTransferObject);
		}

		/// <summary>
		/// Purpose: Reads a page of records from table App_Settings by search criteria.
		/// </summary>
		/// <returns>Returns a collection of AppSettingss</returns>
		public virtual ICollection ReadAppSettingssByPage(AppSettingsDO appSettingsDO) {
			IList appSettingss = new ArrayList();
			IDataReader reader = null;
			try {
				IDataCommand command = dataSource.GetCommand(PagingDO.READ_BY_PAGE);
				command.SetParameter(PagingDO.PAGING_TABLE_NAME, TABLE_NAME);
				command.SetParameter(PagingDO.PAGING_PRIMARY_KEY, AppSettingsDAO.APP_SETTINGS_ID);
				command.SetParameter(PagingDO.PAGING_FIELDS, "*");
				command.SetParameter(PagingDO.PAGING_ORDER, AppSettingsDAO.APP_SETTINGS_ID);
				command.SetParameter(PagingDO.PAGING_PAGE_SIZE, appSettingsDO.Paging.PageSize);
				command.SetParameter(PagingDO.PAGING_PAGE_INDEX, appSettingsDO.Paging.PageIndex);
				command.SetParameter(PagingDO.PAGING_FILTER, AppSettingsDAO.APP_SETTINGS_ID + " > 1");
				command.SetParameter(LANGUAGE, appSettingsDO.Language);
				reader = command.ExecuteReader();
		
				// populate the detail object from the SQL Server Data Reader
				while (reader.Read()) {
					appSettingss.Add(this.MapToDataTransferObject(reader));
				}
				if (reader.NextResult()) {
					reader.Read();
					if (appSettingss.Count < 1) {
						appSettingss.Add(new AppSettingsDO());
					}
					((AppSettingsDO)appSettingss[0]).Paging.TotalRecords = reader.GetInt32(0);
				}
				ProcessResult(reader, appSettingsDO, logCategory);
			} catch (Exception ex) {
				string errorMessage = Resource.GetErrorMessage(MessageKeys.APP_SETTINGS_CANNOT_READ_BY_PAGE, appSettingsDO.Language, ex);
				throw new DataAccessException(errorMessage, ex, logCategory);
			} finally {
				if (reader != null) {
					reader.Close();
				}
			}
			return appSettingss;
		}

	}
}
