///////////////////////////////////////////////////////////////////////////
// Description: Data Access class for the 'Translation_Detail'
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
	/// Purpose: Data Access class for the 'Translation_Detail'.
	/// </summary>
	public class TranslationDetailDAO : TranslationDetailDAOBase {
		private static readonly object logCategory = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;
		// Stored Procedure Name Constants

		// Field Name Constants

		/// <summary>
		/// Purpose: Base Class constructor.
		/// </summary>
		public TranslationDetailDAO() : base() {
		}

		/// <summary>
		/// Purpose: Creates a new record in Translation_Detail table and returns the generated primary key by reference.
		/// </summary>
		/// <param name = "translationDetailDO">TranslationDetailDO translationDetailDO</param>
		public override TranslationDetailDO CreateTranslationDetail(TranslationDetailDO translationDetailDO) {
			return base.CreateTranslationDetail(translationDetailDO);
		}

		/// <summary>
		/// Purpose: Update a record in Translation_Detail.
		/// </summary>
		/// <param name = "translationDetailDO">TranslationDetailDO translationDetailDO</param>
		public override void UpdateTranslationDetail(TranslationDetailDO translationDetailDO) {
			base.UpdateTranslationDetail(translationDetailDO);
		}

		/// <summary>
		/// Purpose: Delete a record in Translation_Detail.
		/// </summary>
		/// <param name = "translationDetailDO">TranslationDetailDO translationDetail</param>
		public override void DeleteTranslationDetail(TranslationDetailDO translationDetailDO) {
			base.DeleteTranslationDetail(translationDetailDO);
		}

		/// <summary>
		/// Purpose: RetrieveFamilySearchData a record in Translation_Detail table by primary key.
		/// </summary>
		/// <param name = "translationDetailDO">TranslationDetailDO translationDetailDO.</param>
		public override TranslationDetailDO ReadTranslationDetail(TranslationDetailDO translationDetailDO) {
			return base.ReadTranslationDetail(translationDetailDO);
		}

		/// <summary>
		/// Purpose: Retrieves all records in Translation_Detail table.
		/// </summary>
		/// <param name = "dataTransferObject">DataTransferObject dataTransferObject.</param>
		public override ICollection ReadTranslationDetailsAll(DataTransferObject dataTransferObject) {
			return base.ReadTranslationDetailsAll(dataTransferObject);
		}

		/// <summary>
		/// Purpose: Retrieves all records from Translation_Detail table for populating a list control.
		/// </summary>
		/// <param name = "dataTransferObject">DataTransferObject dataTransferObject</param>
		public override ICollection ReadTranslationDetailsAllList(DataTransferObject dataTransferObject) {
			return base.ReadTranslationDetailsAllList(dataTransferObject);
		}

		/// <summary>
		/// Purpose: Reads a page of records from table Translation_Detail by search criteria.
		/// </summary>
		/// <returns>Returns a collection of TranslationDetails</returns>
		public virtual ICollection ReadTranslationDetailsByPage(TranslationDetailDO translationDetailDO) {
			IList translationDetails = new ArrayList();
			IDataReader reader = null;
			try {
				IDataCommand command = dataSource.GetCommand(PagingDO.READ_BY_PAGE);
				command.SetParameter(PagingDO.PAGING_TABLE_NAME, TABLE_NAME);
				command.SetParameter(PagingDO.PAGING_PRIMARY_KEY, TranslationDetailDAO.TRANSLATION_DETAIL_ID);
				command.SetParameter(PagingDO.PAGING_FIELDS, "*");
				command.SetParameter(PagingDO.PAGING_ORDER, TranslationDetailDAO.TRANSLATION_DETAIL_ID);
				command.SetParameter(PagingDO.PAGING_PAGE_SIZE, translationDetailDO.Paging.PageSize);
				command.SetParameter(PagingDO.PAGING_PAGE_INDEX, translationDetailDO.Paging.PageIndex);
				command.SetParameter(PagingDO.PAGING_FILTER, TranslationDetailDAO.TRANSLATION_DETAIL_ID + " > 1");
				command.SetParameter(LANGUAGE, translationDetailDO.Language);
				reader = command.ExecuteReader();
		
				// populate the detail object from the SQL Server Data Reader
				while (reader.Read()) {
					translationDetails.Add(this.MapToDataTransferObject(reader));
				}
				if (reader.NextResult()) {
					reader.Read();
					if (translationDetails.Count < 1) {
						translationDetails.Add(new TranslationDetailDO());
					}
					((TranslationDetailDO)translationDetails[0]).Paging.TotalRecords = reader.GetInt32(0);
				}
				ProcessResult(reader, translationDetailDO, logCategory);
			} catch (Exception ex) {
				string errorMessage = Resource.GetErrorMessage(MessageKeys.TRANSLATION_DETAIL_CANNOT_READ_BY_PAGE, translationDetailDO.Language, ex);
				throw new DataAccessException(errorMessage, ex, logCategory);
			} finally {
				if (reader != null) {
					reader.Close();
				}
			}
			return translationDetails;
		}

	}
}
