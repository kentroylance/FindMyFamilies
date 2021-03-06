///////////////////////////////////////////////////////////////////////////
// Description: AdminServicesBase Class
// Generated by FindMyFamilies Generator
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;

using FindMyFamilies.Data;
using FindMyFamilies.Util;
using FindMyFamilies.Exceptions;
using FindMyFamilies.BusinessObject;

namespace findmyfamilies.Services {

	/// <summary>
	/// Purpose: Services Facade Base Class for AdminServicesBase
	/// </summary>
	public class AdminServicesBase {

		/// <summary>
		/// Purpose: Create a NextID
		/// </summary>
		/// <param name = "nextIDDO">NextIDDO nextIDDO</param>
		public virtual NextIDDO CreateNextID(NextIDDO nextIDDO) {
			return NextIDBO.CreateNextID(nextIDDO);
		}
		
		/// <summary>
		/// Purpose: Create NextIDs
		/// </summary>
		/// <param name = "nextIDDOs">ICollection nextIDDOs</param>
		public virtual ICollection CreateNextIDs(IList nextIDDOs) {
			return NextIDBO.CreateNextIDs(nextIDDOs);
		}
		
		private static NextIDBO m_NextIDBO;
		
		public NextIDBO NextIDBO {
			get {
				if (m_NextIDBO == null) {
					AdminServicesBase adminServices = new AdminServicesBase();
					lock(adminServices) {
						m_NextIDBO = new NextIDBO();
					}
				}
				return m_NextIDBO;
			}
		}
		
		/// <summary>
		/// Purpose: Get a NextID with a primary key
		///      Must pass a Table_ID in the data object
		/// </summary>
		/// <param name = "nextIDDO">NextIDDO nextIDDO</param>
		public virtual NextIDDO ReadNextID(NextIDDO nextIDDO) {
			return NextIDBO.ReadNextID(nextIDDO);
		}
		
		/// <summary>
		/// Purpose: Read all NextIDs
		/// </summary>
		/// <returns>Returns a collection of NextID data objects</returns>
		/// <param name = "dataTransferObject">DataTransferObject dataTransferObject.</param>
		public virtual ICollection ReadNextIDsAll(DataTransferObject dataTransferObject) {
			return NextIDBO.ReadNextIDsAll(dataTransferObject);
		}
		
		/// <summary>
		/// Purpose: Read NextIDs by page
		/// </summary>
		/// <returns>Returns NextID data objects by page</returns>
		/// <param name = "nextIDDO">NextIDDO nextIDDO</param>
		public virtual ICollection ReadNextIDsByPage(NextIDDO nextIDDO) {
			return NextIDBO.ReadNextIDsByPage(nextIDDO);
		}
		
		/// <summary>
		/// Purpose: Update a NextID
		/// </summary>
		/// <param name = "nextIDDO">NextIDDO nextIDDO</param>
		public virtual void UpdateNextID(NextIDDO nextIDDO) {
			NextIDBO.UpdateNextID(nextIDDO);
		}
		
		/// <summary>
		/// Purpose: Update NextIDs
		/// </summary>
		/// <param name = "nextIDDOs">ICollection nextIDDOs</param>
		public virtual void UpdateNextIDs(IList nextIDDOs) {
			NextIDBO.UpdateNextIDs(nextIDDOs);
		}
		
		/// <summary>
		/// Purpose: Delete a NextID
		/// </summary>
		/// <param name = "nextIDDO">NextIDDO nextIDDO</param>
		public virtual void DeleteNextID(NextIDDO nextIDDO) {
			NextIDBO.DeleteNextID(nextIDDO);
		}

		/// <summary>
		/// Purpose: Delete NextIDs
		/// </summary>
		/// <param name = "nextIDDOs">ICollection nextIDDOs</param>
		public virtual void DeleteNextIDs(IList nextIDDOs) {
			NextIDBO.DeleteNextIDs(nextIDDOs);
		}
		
		/// <summary>
		/// Purpose: Create a Translation
		/// </summary>
		/// <param name = "translationDO">TranslationDO translationDO</param>
		public virtual TranslationDO CreateTranslation(TranslationDO translationDO) {
			return TranslationBO.CreateTranslation(translationDO);
		}
		
		/// <summary>
		/// Purpose: Create Translations
		/// </summary>
		/// <param name = "translationDOs">ICollection translationDOs</param>
		public virtual ICollection CreateTranslations(IList translationDOs) {
			return TranslationBO.CreateTranslations(translationDOs);
		}
		
		private static TranslationBO m_TranslationBO;
		
		public TranslationBO TranslationBO {
			get {
				if (m_TranslationBO == null) {
					AdminServicesBase adminServices = new AdminServicesBase();
					lock(adminServices) {
						m_TranslationBO = new TranslationBO();
					}
				}
				return m_TranslationBO;
			}
		}
		
		/// <summary>
		/// Purpose: Get a Translation with a primary key
		///      Must pass a Translation_ID in the data object
		/// </summary>
		/// <param name = "translationDO">TranslationDO translationDO</param>
		public virtual TranslationDO ReadTranslation(TranslationDO translationDO) {
			return TranslationBO.ReadTranslation(translationDO);
		}
		
		/// <summary>
		/// Purpose: Read all Translations
		/// </summary>
		/// <returns>Returns a collection of Translation data objects</returns>
		/// <param name = "dataTransferObject">DataTransferObject dataTransferObject.</param>
		public virtual ICollection ReadTranslationsAll(DataTransferObject dataTransferObject) {
			return TranslationBO.ReadTranslationsAll(dataTransferObject);
		}
		
		/// <summary>
		/// Purpose: Read all Translations for a list
		/// </summary>
		/// <returns>Returns a collection of ListItem data objects</returns>
		/// <param name = "dataTransferObject">DataTransferObject dataTransferObject.</param>
		public virtual ICollection ReadTranslationsAllList(DataTransferObject dataTransferObject) {
			return TranslationBO.ReadTranslationsAllList(dataTransferObject);
		}
		
		/// <summary>
		/// Purpose: Read Translations by page
		/// </summary>
		/// <returns>Returns Translation data objects by page</returns>
		/// <param name = "translationDO">TranslationDO translationDO</param>
		public virtual ICollection ReadTranslationsByPage(TranslationDO translationDO) {
			return TranslationBO.ReadTranslationsByPage(translationDO);
		}
		
		/// <summary>
		/// Purpose: Update a Translation
		/// </summary>
		/// <param name = "translationDO">TranslationDO translationDO</param>
		public virtual void UpdateTranslation(TranslationDO translationDO) {
			TranslationBO.UpdateTranslation(translationDO);
		}
		
		/// <summary>
		/// Purpose: Update Translations
		/// </summary>
		/// <param name = "translationDOs">ICollection translationDOs</param>
		public virtual void UpdateTranslations(IList translationDOs) {
			TranslationBO.UpdateTranslations(translationDOs);
		}
		
		/// <summary>
		/// Purpose: Delete a Translation
		/// </summary>
		/// <param name = "translationDO">TranslationDO translationDO</param>
		public virtual void DeleteTranslation(TranslationDO translationDO) {
			TranslationBO.DeleteTranslation(translationDO);
		}

		/// <summary>
		/// Purpose: Delete Translations
		/// </summary>
		/// <param name = "translationDOs">ICollection translationDOs</param>
		public virtual void DeleteTranslations(IList translationDOs) {
			TranslationBO.DeleteTranslations(translationDOs);
		}
		
		/// <summary>
		/// Purpose: Create a TranslationDetail
		/// </summary>
		/// <param name = "translationDetailDO">TranslationDetailDO translationDetailDO</param>
		public virtual TranslationDetailDO CreateTranslationDetail(TranslationDetailDO translationDetailDO) {
			return TranslationDetailBO.CreateTranslationDetail(translationDetailDO);
		}
		
		/// <summary>
		/// Purpose: Create TranslationDetails
		/// </summary>
		/// <param name = "translationDetailDOs">ICollection translationDetailDOs</param>
		public virtual ICollection CreateTranslationDetails(IList translationDetailDOs) {
			return TranslationDetailBO.CreateTranslationDetails(translationDetailDOs);
		}
		
		private static TranslationDetailBO m_TranslationDetailBO;
		
		public TranslationDetailBO TranslationDetailBO {
			get {
				if (m_TranslationDetailBO == null) {
					AdminServicesBase adminServices = new AdminServicesBase();
					lock(adminServices) {
						m_TranslationDetailBO = new TranslationDetailBO();
					}
				}
				return m_TranslationDetailBO;
			}
		}
		
		/// <summary>
		/// Purpose: Get a TranslationDetail with a primary key
		///      Must pass a Translation_Detail_ID in the data object
		/// </summary>
		/// <param name = "translationDetailDO">TranslationDetailDO translationDetailDO</param>
		public virtual TranslationDetailDO ReadTranslationDetail(TranslationDetailDO translationDetailDO) {
			return TranslationDetailBO.ReadTranslationDetail(translationDetailDO);
		}
		
		/// <summary>
		/// Purpose: Read all TranslationDetails
		/// </summary>
		/// <returns>Returns a collection of TranslationDetail data objects</returns>
		/// <param name = "dataTransferObject">DataTransferObject dataTransferObject.</param>
		public virtual ICollection ReadTranslationDetailsAll(DataTransferObject dataTransferObject) {
			return TranslationDetailBO.ReadTranslationDetailsAll(dataTransferObject);
		}
		
		/// <summary>
		/// Purpose: Read all TranslationDetails for a list
		/// </summary>
		/// <returns>Returns a collection of ListItem data objects</returns>
		/// <param name = "dataTransferObject">DataTransferObject dataTransferObject.</param>
		public virtual ICollection ReadTranslationDetailsAllList(DataTransferObject dataTransferObject) {
			return TranslationDetailBO.ReadTranslationDetailsAllList(dataTransferObject);
		}
		
		/// <summary>
		/// Purpose: Read TranslationDetails by page
		/// </summary>
		/// <returns>Returns TranslationDetail data objects by page</returns>
		/// <param name = "translationDetailDO">TranslationDetailDO translationDetailDO</param>
		public virtual ICollection ReadTranslationDetailsByPage(TranslationDetailDO translationDetailDO) {
			return TranslationDetailBO.ReadTranslationDetailsByPage(translationDetailDO);
		}
		
		/// <summary>
		/// Purpose: Update a TranslationDetail
		/// </summary>
		/// <param name = "translationDetailDO">TranslationDetailDO translationDetailDO</param>
		public virtual void UpdateTranslationDetail(TranslationDetailDO translationDetailDO) {
			TranslationDetailBO.UpdateTranslationDetail(translationDetailDO);
		}
		
		/// <summary>
		/// Purpose: Update TranslationDetails
		/// </summary>
		/// <param name = "translationDetailDOs">ICollection translationDetailDOs</param>
		public virtual void UpdateTranslationDetails(IList translationDetailDOs) {
			TranslationDetailBO.UpdateTranslationDetails(translationDetailDOs);
		}
		
		/// <summary>
		/// Purpose: Delete a TranslationDetail
		/// </summary>
		/// <param name = "translationDetailDO">TranslationDetailDO translationDetailDO</param>
		public virtual void DeleteTranslationDetail(TranslationDetailDO translationDetailDO) {
			TranslationDetailBO.DeleteTranslationDetail(translationDetailDO);
		}

		/// <summary>
		/// Purpose: Delete TranslationDetails
		/// </summary>
		/// <param name = "translationDetailDOs">ICollection translationDetailDOs</param>
		public virtual void DeleteTranslationDetails(IList translationDetailDOs) {
			TranslationDetailBO.DeleteTranslationDetails(translationDetailDOs);
		}
		
		/// <summary>
		/// Purpose: Create a TranslationMaster
		/// </summary>
		/// <param name = "translationMasterDO">TranslationMasterDO translationMasterDO</param>
		public virtual TranslationMasterDO CreateTranslationMaster(TranslationMasterDO translationMasterDO) {
			return TranslationMasterBO.CreateTranslationMaster(translationMasterDO);
		}
		
		/// <summary>
		/// Purpose: Create TranslationMasters
		/// </summary>
		/// <param name = "translationMasterDOs">ICollection translationMasterDOs</param>
		public virtual ICollection CreateTranslationMasters(IList translationMasterDOs) {
			return TranslationMasterBO.CreateTranslationMasters(translationMasterDOs);
		}
		
		private static TranslationMasterBO m_TranslationMasterBO;
		
		public TranslationMasterBO TranslationMasterBO {
			get {
				if (m_TranslationMasterBO == null) {
					AdminServicesBase adminServices = new AdminServicesBase();
					lock(adminServices) {
						m_TranslationMasterBO = new TranslationMasterBO();
					}
				}
				return m_TranslationMasterBO;
			}
		}
		
		/// <summary>
		/// Purpose: Get a TranslationMaster with a primary key
		///      Must pass a Translation_Master_ID in the data object
		/// </summary>
		/// <param name = "translationMasterDO">TranslationMasterDO translationMasterDO</param>
		public virtual TranslationMasterDO ReadTranslationMaster(TranslationMasterDO translationMasterDO) {
			return TranslationMasterBO.ReadTranslationMaster(translationMasterDO);
		}
		
		/// <summary>
		/// Purpose: Read all TranslationMasters
		/// </summary>
		/// <returns>Returns a collection of TranslationMaster data objects</returns>
		/// <param name = "dataTransferObject">DataTransferObject dataTransferObject.</param>
		public virtual ICollection ReadTranslationMastersAll(DataTransferObject dataTransferObject) {
			return TranslationMasterBO.ReadTranslationMastersAll(dataTransferObject);
		}
		
		/// <summary>
		/// Purpose: Read all TranslationMasters for a list
		/// </summary>
		/// <returns>Returns a collection of ListItem data objects</returns>
		/// <param name = "dataTransferObject">DataTransferObject dataTransferObject.</param>
		public virtual ICollection ReadTranslationMastersAllList(DataTransferObject dataTransferObject) {
			return TranslationMasterBO.ReadTranslationMastersAllList(dataTransferObject);
		}
		
		/// <summary>
		/// Purpose: Read TranslationMasters by page
		/// </summary>
		/// <returns>Returns TranslationMaster data objects by page</returns>
		/// <param name = "translationMasterDO">TranslationMasterDO translationMasterDO</param>
		public virtual ICollection ReadTranslationMastersByPage(TranslationMasterDO translationMasterDO) {
			return TranslationMasterBO.ReadTranslationMastersByPage(translationMasterDO);
		}
		
		/// <summary>
		/// Purpose: Update a TranslationMaster
		/// </summary>
		/// <param name = "translationMasterDO">TranslationMasterDO translationMasterDO</param>
		public virtual void UpdateTranslationMaster(TranslationMasterDO translationMasterDO) {
			TranslationMasterBO.UpdateTranslationMaster(translationMasterDO);
		}
		
		/// <summary>
		/// Purpose: Update TranslationMasters
		/// </summary>
		/// <param name = "translationMasterDOs">ICollection translationMasterDOs</param>
		public virtual void UpdateTranslationMasters(IList translationMasterDOs) {
			TranslationMasterBO.UpdateTranslationMasters(translationMasterDOs);
		}
		
		/// <summary>
		/// Purpose: Delete a TranslationMaster
		/// </summary>
		/// <param name = "translationMasterDO">TranslationMasterDO translationMasterDO</param>
		public virtual void DeleteTranslationMaster(TranslationMasterDO translationMasterDO) {
			TranslationMasterBO.DeleteTranslationMaster(translationMasterDO);
		}

		/// <summary>
		/// Purpose: Delete TranslationMasters
		/// </summary>
		/// <param name = "translationMasterDOs">ICollection translationMasterDOs</param>
		public virtual void DeleteTranslationMasters(IList translationMasterDOs) {
			TranslationMasterBO.DeleteTranslationMasters(translationMasterDOs);
		}
		
				/// Purpose: Create a Email
		/// </summary>
		/// <param name = "emailDO">EmailDO emailDO</param>
		public virtual EmailDO CreateEmail(EmailDO emailDO, ref SessionDO session) {
			return EmailBO.CreateEmail(emailDO, ref session);
		}
		
		/// <summary>
		/// Purpose: Create Emails
		/// </summary>
		/// <param name = "emailDOs">ICollection emailDOs</param>
		public virtual ICollection CreateEmails(IList emailDOs) {
			return EmailBO.CreateEmails(emailDOs);
		}
		
		private static EmailBO m_EmailBO;
		
		public EmailBO EmailBO {
			get {
				if (m_EmailBO == null) {
					AdminServicesBase adminServices = new AdminServicesBase();
					lock(adminServices) {
						m_EmailBO = new EmailBO();
					}
				}
				return m_EmailBO;
			}
		}
		
		/// <summary>
		/// Purpose: Get a Email with a primary key
		///      Must pass a Email_ID in the data object
		/// </summary>
		/// <param name = "emailDO">EmailDO emailDO</param>
		public virtual EmailDO ReadEmail(EmailDO emailDO) {
			return EmailBO.ReadEmail(emailDO);
		}
		
		/// <summary>
		/// Purpose: Read all Emails
		/// </summary>
		/// <returns>Returns a collection of Email data objects</returns>
		/// <param name = "dataTransferObject">DataTransferObject dataTransferObject.</param>
		public virtual ICollection ReadEmailsAll(DataTransferObject dataTransferObject) {
			return EmailBO.ReadEmailsAll(dataTransferObject);
		}
		
		/// <summary>
		/// Purpose: Read all Emails for a list
		/// </summary>
		/// <returns>Returns a collection of ListItem data objects</returns>
		/// <param name = "dataTransferObject">DataTransferObject dataTransferObject.</param>
		public virtual ICollection ReadEmailsAllList(DataTransferObject dataTransferObject) {
			return EmailBO.ReadEmailsAllList(dataTransferObject);
		}
		
		/// <summary>
		/// Purpose: Read Emails by page
		/// </summary>
		/// <returns>Returns Email data objects by page</returns>
		/// <param name = "emailDO">EmailDO emailDO</param>
		public virtual ICollection ReadEmailsByPage(EmailDO emailDO) {
			return EmailBO.ReadEmailsByPage(emailDO);
		}
		
		/// <summary>
		/// Purpose: Update a Email
		/// </summary>
		/// <param name = "emailDO">EmailDO emailDO</param>
		public virtual void UpdateEmail(EmailDO emailDO) {
			EmailBO.UpdateEmail(emailDO);
		}
		
		/// <summary>
		/// Purpose: Update Emails
		/// </summary>
		/// <param name = "emailDOs">ICollection emailDOs</param>
		public virtual void UpdateEmails(IList emailDOs) {
			EmailBO.UpdateEmails(emailDOs);
		}
		
		/// <summary>
		/// Purpose: Delete a Email
		/// </summary>
		/// <param name = "emailDO">EmailDO emailDO</param>
		public virtual void DeleteEmail(EmailDO emailDO) {
			EmailBO.DeleteEmail(emailDO);
		}

		/// <summary>
		/// Purpose: Delete Emails
		/// </summary>
		/// <param name = "emailDOs">ICollection emailDOs</param>
		public virtual void DeleteEmails(IList emailDOs) {
			EmailBO.DeleteEmails(emailDOs);
		}
		
	}
}
