///////////////////////////////////////////////////////////////////////////
// Description: Business Object Base class for the 'Translation'
// Generated by FindMyFamilies Generator
///////////////////////////////////////////////////////////////////////////
using System;
using System.Reflection;
using System.Collections;
using System.EnterpriseServices;

using FindMyFamilies.Data;
using FindMyFamilies.Util;
using FindMyFamilies.Exceptions;
using FindMyFamilies.Transactions;
using FindMyFamilies.DataAccess;

// Please do not modify any code in this auto generated class.  This code will be overwritten when the generator is executed.
namespace FindMyFamilies.BusinessObject {

	/// <summary>
	/// Purpose: Business Object base class for Translation
	/// </summary>
	public class TranslationBOBase : BusinessObjectBase {
		// Constant Declarations
		private static readonly object logCategory = MethodBase.GetCurrentMethod().DeclaringType;
		private static TranslationDAO m_TranslationDAO;

		// Property Declarations
		public static TranslationDAO TranslationDAO {
			get {
				if (m_TranslationDAO == null) {
					TranslationBOBase translationBO = new TranslationBOBase();
					lock(translationBO) {
						m_TranslationDAO = new TranslationDAO();
					}
				}
				return m_TranslationDAO;
			}
		}

		public virtual TranslationDO CreateTranslation(TranslationDO translationDO) {
			return TranslationDAO.CreateTranslation(translationDO);
		}

		public virtual TranslationDO ReadTranslation(TranslationDO translationDO) {
			return TranslationDAO.ReadTranslation(translationDO);
		}

		public virtual ICollection ReadTranslationsAll(DataTransferObject dataTransferObject) {
			return TranslationDAO.ReadTranslationsAll(dataTransferObject);
		}

		public virtual ICollection ReadTranslationsAllList(DataTransferObject dataTransferObject) {
			return TranslationDAO.ReadTranslationsAllList(dataTransferObject);
		}

		public virtual void UpdateTranslation(TranslationDO translationDO) {
			TranslationDAO.UpdateTranslation(translationDO);
		}

		public virtual void DeleteTranslation(TranslationDO translationDO) {
			TranslationDAO.DeleteTranslation(translationDO);
		}

		public virtual ICollection ReadTranslationsByPage(TranslationDO translationDO) {
			return TranslationDAO.ReadTranslationsByPage(translationDO);
		}

	}
}
