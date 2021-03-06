///////////////////////////////////////////////////////////////////////////
// Description: Business Object class for the 'Translation'
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

namespace FindMyFamilies.BusinessObject {

	/// <summary>
	/// Purpose: Business Object class for Translation
	/// </summary>
	public class TranslationBO : FindMyFamilies.BusinessObject.TranslationBOBase {
		// Constant Declarations

		private static readonly object logCategory = MethodBase.GetCurrentMethod().DeclaringType;

		// Property Declarations

		// Property Getters/Setters

		/// <summary>
		/// Purpose: Class constructor.
		/// </summary>
		public TranslationBO() : base() {
		}

		/// <summary>
		/// Purpose: Create Translation records in the database
		/// </summary>
		/// <returns>Returns a list of Translations by reference with their new primary key</returns>
		/// <param name = "translations">IList translations.</param>
		public ICollection CreateTranslations(IList translations) {
			if (translations != null) {
				if (translations.Count > 0) {
					TransactionContext context = TransactionContextFactory.GetContext(TransactionAffinity.Required);
					try {
						context.Enter();
						for (int i = 0; i < translations.Count; i++) {
							this.CreateTranslation((TranslationDO)translations[i]);
						}
						context.VoteCommit();
					} catch(Exception ex) {
						context.VoteRollback();
						string errorMessage = Resource.GetErrorMessage(MessageKeys.TRANSLATION_CANNOT_CREATE, base.GetLanguage(translations), ex);
						throw new DataAccessException(errorMessage, ex, logCategory);
					} finally {
						context.Exit();
					}
				} else {
					this.CreateTranslation((TranslationDO)translations[0]);
				}
			} else {
				string errorMessage = Resource.GetErrorMessage(MessageKeys.TRANSLATION_CANNOT_CREATE, base.GetLanguage(translations));
				throw new DataAccessException(errorMessage, null, logCategory);
			}
			return translations;
		}

		/// <summary>
		/// Purpose: Create a Translation record in the database
		/// </summary>
		/// <returns>Returns a Translation by reference with its new primary key</returns>
		/// <param name = "translationDO">TranslationDO translationDO.</param>
		public override TranslationDO CreateTranslation(TranslationDO translationDO) {
			if (IsValid(this, MethodBase.GetCurrentMethod().Name, translationDO)) {
				translationDO = base.CreateTranslation(translationDO);
			}
			return translationDO;
		}

		/// <summary>
		/// Purpose: Retrieves a Translation record by (Translation_ID)
		/// </summary>
		/// <returns>Returns a Translation Data Object by reference</returns>
		/// <param name = "translationDO">TranslationDO translationDO.</param>
		public override TranslationDO ReadTranslation(TranslationDO translationDO) {
			if (IsValid(this, MethodBase.GetCurrentMethod().Name, translationDO)) {
				translationDO = base.ReadTranslation(translationDO);
			}
			return translationDO;
		}

		/// <summary>
		/// Purpose: Retrieves all Translations
		/// </summary>
		/// <returns>Returns a Collection of TranslationDO Data Objects in a ICollection</returns>
		/// <param name = "dataTransferObject">DataTransferObject dataTransferObject</param>
		public override ICollection ReadTranslationsAll(DataTransferObject dataTransferObject) {
			ICollection translations = new ArrayList();
			if (IsValid(this, MethodBase.GetCurrentMethod().Name, dataTransferObject)) {
				translations = base.ReadTranslationsAll(dataTransferObject);
			}
			return translations;
		}

		/// <summary>
		/// Purpose: Retrieves all Translations for a list control
		/// </summary>
		/// <returns>Returns a Collection of ListItem Data Objects in a ICollection</returns>
		/// <param name = "dataTransferObject">DataTransferObject dataTransferObject</param>
		public override ICollection ReadTranslationsAllList(DataTransferObject dataTransferObject) {
			ICollection translations = new ArrayList();
			if (IsValid(this, MethodBase.GetCurrentMethod().Name, dataTransferObject)) {
				translations = base.ReadTranslationsAllList(dataTransferObject);
			}
			return translations;
		}

		/// <summary>
		/// Purpose: RetrieveFamilySearchData Translations by page
		/// </summary>
		/// <returns>Returns a Collection of TranslationDO Data Objects in a ICollection</returns>
		/// <param name = "translationDO">TranslationDO translationDO.</param>
		public override ICollection ReadTranslationsByPage(TranslationDO translationDO) {
			ICollection translations = new ArrayList();
			if (IsValid(this, MethodBase.GetCurrentMethod().Name, translationDO)) {
				translations = base.ReadTranslationsByPage(translationDO);
			}
			return translations;
		}

		/// <summary>
		/// Purpose: Update Translation records in the database
		/// </summary>
		/// <param name = "translations">IList translations.</param>
		public void UpdateTranslations(IList translations) {
			if (translations != null) {
				if (translations.Count > 0) {
					TransactionContext context = TransactionContextFactory.GetContext(TransactionAffinity.Required);
					try {
						context.Enter();
						for (int i = 0; i < translations.Count; i++) {
							this.UpdateTranslation((TranslationDO)translations[i]);
						}
						context.VoteCommit();
					} catch(Exception ex) {
						context.VoteRollback();
						string errorMessage = Resource.GetErrorMessage(MessageKeys.TRANSLATION_CANNOT_UPDATE, base.GetLanguage(translations), ex);
						throw new DataAccessException(errorMessage, ex, logCategory);
					} finally {
						context.Exit();
					}
				} else {
					this.UpdateTranslation((TranslationDO)translations[0]);
				}
			} else {
				string errorMessage = Resource.GetErrorMessage(MessageKeys.TRANSLATION_CANNOT_UPDATE, base.GetLanguage(translations));
				throw new DataAccessException(errorMessage, null, logCategory);
			}
		}

//		/// <summary>
//		/// Purpose: Update records in Translation table.
//		/// </summary>
//		/// <param name = translations>ICollection translations</param>
//		public override void UpdateTranslations(ICollection translations) {
//			base.UpdateTranslations(translations);
//		}
		/// <summary>
		/// Purpose: Update a Translation record in the database
		/// </summary>
		/// <param name = "translationDO">TranslationDO translationDO.</param>
		public override void UpdateTranslation(TranslationDO translationDO) {
			if (IsValid(this, MethodBase.GetCurrentMethod().Name, translationDO)) {
				base.UpdateTranslation(translationDO);
			}
		}

		/// <summary>
		/// Purpose: Delete Translation records in the database
		/// </summary>
		/// <param name = "translations">IList translations.</param>
		public void DeleteTranslations(IList translations) {
			if (translations != null) {
				if (translations.Count > 0) {
					TransactionContext context = TransactionContextFactory.GetContext(TransactionAffinity.Required);
					try {
						context.Enter();
						for (int i = 0; i < translations.Count; i++) {
							this.DeleteTranslation((TranslationDO)translations[i]);
						}
						context.VoteCommit();
					} catch(Exception ex) {
						context.VoteRollback();
						string errorMessage = Resource.GetErrorMessage(MessageKeys.TRANSLATION_CANNOT_DELETE, base.GetLanguage(translations), ex);
						throw new DataAccessException(errorMessage, ex, logCategory);
					} finally {
						context.Exit();
					}
				} else {
					this.DeleteTranslation((TranslationDO)translations[0]);
				}
			} else {
				string errorMessage = Resource.GetErrorMessage(MessageKeys.TRANSLATION_CANNOT_DELETE, base.GetLanguage(translations));
				throw new DataAccessException(errorMessage, null, logCategory);
			}
		}

		/// <summary>
		/// Purpose: Delete a Translation record in the database
		/// </summary>
		/// <param name = "translationDO">TranslationDO translationDO.</param>
		public override void DeleteTranslation(TranslationDO translationDO) {
			if (IsValid(this, MethodBase.GetCurrentMethod().Name, translationDO)) {
				base.DeleteTranslation(translationDO);
			}
		}

	}
}
