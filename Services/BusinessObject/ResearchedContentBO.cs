///////////////////////////////////////////////////////////////////////////
// Description: Business Object class for the 'Researched_Content'
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
	/// Purpose: Business Object class for ResearchedContent
	/// </summary>
	public class ResearchedContentBO : FindMyFamilies.BusinessObject.ResearchedContentBOBase {
		// Constant Declarations

		private static readonly object logCategory = MethodBase.GetCurrentMethod().DeclaringType;

		// Property Declarations

		// Property Getters/Setters

		/// <summary>
		/// Purpose: Class constructor.
		/// </summary>
		public ResearchedContentBO() : base() {
		}

		/// <summary>
		/// Purpose: Create ResearchedContent records in the database
		/// </summary>
		/// <returns>Returns a list of ResearchedContents by reference with their new primary key</returns>
		/// <param name = "researchedContents">IList researchedContents.</param>
		public ICollection CreateResearchedContents(IList researchedContents) {
			if (researchedContents != null) {
				if (researchedContents.Count > 0) {
					TransactionContext context = TransactionContextFactory.GetContext(TransactionAffinity.Required);
					try {
						context.Enter();
						for (int i = 0; i < researchedContents.Count; i++) {
							this.CreateResearchedContent((ResearchedContentDO)researchedContents[i]);
						}
						context.VoteCommit();
					} catch(Exception ex) {
						context.VoteRollback();
						string errorMessage = Resource.GetErrorMessage(MessageKeys.RESEARCHED_CONTENT_CANNOT_CREATE, base.GetLanguage(researchedContents), ex);
						throw new DataAccessException(errorMessage, ex, logCategory);
					} finally {
						context.Exit();
					}
				} else {
					this.CreateResearchedContent((ResearchedContentDO)researchedContents[0]);
				}
			} else {
				string errorMessage = Resource.GetErrorMessage(MessageKeys.RESEARCHED_CONTENT_CANNOT_CREATE, base.GetLanguage(researchedContents));
				throw new DataAccessException(errorMessage, null, logCategory);
			}
			return researchedContents;
		}

		/// <summary>
		/// Purpose: Create a ResearchedContent record in the database
		/// </summary>
		/// <returns>Returns a ResearchedContent by reference with its new primary key</returns>
		/// <param name = "researchedContentDO">ResearchedContentDO researchedContentDO.</param>
		public override ResearchedContentDO CreateResearchedContent(ResearchedContentDO researchedContentDO) {
			if (IsValid(this, MethodBase.GetCurrentMethod().Name, researchedContentDO)) {
				researchedContentDO = base.CreateResearchedContent(researchedContentDO);
			}
			return researchedContentDO;
		}

		/// <summary>
		/// Purpose: Retrieves a ResearchedContent record by (Researched_Content_ID)
		/// </summary>
		/// <returns>Returns a ResearchedContent Data Object by reference</returns>
		/// <param name = "researchedContentDO">ResearchedContentDO researchedContentDO.</param>
		public override ResearchedContentDO ReadResearchedContent(ResearchedContentDO researchedContentDO) {
			if (IsValid(this, MethodBase.GetCurrentMethod().Name, researchedContentDO)) {
				researchedContentDO = base.ReadResearchedContent(researchedContentDO);
			}
			return researchedContentDO;
		}

		/// <summary>
		/// Purpose: Retrieves all ResearchedContents
		/// </summary>
		/// <returns>Returns a Collection of ResearchedContentDO Data Objects in a ICollection</returns>
		/// <param name = "dataTransferObject">DataTransferObject dataTransferObject</param>
		public override ICollection ReadResearchedContentsAll(DataTransferObject dataTransferObject) {
			ICollection researchedContents = new ArrayList();
			if (IsValid(this, MethodBase.GetCurrentMethod().Name, dataTransferObject)) {
				researchedContents = base.ReadResearchedContentsAll(dataTransferObject);
			}
			return researchedContents;
		}

		/// <summary>
		/// Purpose: Retrieves all ResearchedContents for a list control
		/// </summary>
		/// <returns>Returns a Collection of ListItem Data Objects in a ICollection</returns>
		/// <param name = "dataTransferObject">DataTransferObject dataTransferObject</param>
		public override ICollection ReadResearchedContentsAllList(DataTransferObject dataTransferObject) {
			ICollection researchedContents = new ArrayList();
			if (IsValid(this, MethodBase.GetCurrentMethod().Name, dataTransferObject)) {
				researchedContents = base.ReadResearchedContentsAllList(dataTransferObject);
			}
			return researchedContents;
		}

		/// <summary>
		/// Purpose: Retrieve ResearchedContents by page
		/// </summary>
		/// <returns>Returns a Collection of ResearchedContentDO Data Objects in a ICollection</returns>
		/// <param name = "researchedContentDO">ResearchedContentDO researchedContentDO.</param>
		public override ICollection ReadResearchedContentsByPage(ResearchedContentDO researchedContentDO) {
			ICollection researchedContents = new ArrayList();
			if (IsValid(this, MethodBase.GetCurrentMethod().Name, researchedContentDO)) {
				researchedContents = base.ReadResearchedContentsByPage(researchedContentDO);
			}
			return researchedContents;
		}

		/// <summary>
		/// Purpose: Update ResearchedContent records in the database
		/// </summary>
		/// <param name = "researchedContents">IList researchedContents.</param>
		public void UpdateResearchedContents(IList researchedContents) {
			if (researchedContents != null) {
				if (researchedContents.Count > 0) {
					TransactionContext context = TransactionContextFactory.GetContext(TransactionAffinity.Required);
					try {
						context.Enter();
						for (int i = 0; i < researchedContents.Count; i++) {
							this.UpdateResearchedContent((ResearchedContentDO)researchedContents[i]);
						}
						context.VoteCommit();
					} catch(Exception ex) {
						context.VoteRollback();
						string errorMessage = Resource.GetErrorMessage(MessageKeys.RESEARCHED_CONTENT_CANNOT_UPDATE, base.GetLanguage(researchedContents), ex);
						throw new DataAccessException(errorMessage, ex, logCategory);
					} finally {
						context.Exit();
					}
				} else {
					this.UpdateResearchedContent((ResearchedContentDO)researchedContents[0]);
				}
			} else {
				string errorMessage = Resource.GetErrorMessage(MessageKeys.RESEARCHED_CONTENT_CANNOT_UPDATE, base.GetLanguage(researchedContents));
				throw new DataAccessException(errorMessage, null, logCategory);
			}
		}

		/// <summary>
		/// Purpose: Update records in ResearchedContent table.
		/// </summary>
		/// <param name = researchedContents>ICollection researchedContents</param>
		public override void UpdateResearchedContents(ICollection researchedContents) {
			base.UpdateResearchedContents(researchedContents);
		}
		/// <summary>
		/// Purpose: Update a ResearchedContent record in the database
		/// </summary>
		/// <param name = "researchedContentDO">ResearchedContentDO researchedContentDO.</param>
		public override void UpdateResearchedContent(ResearchedContentDO researchedContentDO) {
			if (IsValid(this, MethodBase.GetCurrentMethod().Name, researchedContentDO)) {
				base.UpdateResearchedContent(researchedContentDO);
			}
		}

		/// <summary>
		/// Purpose: Delete ResearchedContent records in the database
		/// </summary>
		/// <param name = "researchedContents">IList researchedContents.</param>
		public void DeleteResearchedContents(IList researchedContents) {
			if (researchedContents != null) {
				if (researchedContents.Count > 0) {
					TransactionContext context = TransactionContextFactory.GetContext(TransactionAffinity.Required);
					try {
						context.Enter();
						for (int i = 0; i < researchedContents.Count; i++) {
							this.DeleteResearchedContent((ResearchedContentDO)researchedContents[i]);
						}
						context.VoteCommit();
					} catch(Exception ex) {
						context.VoteRollback();
						string errorMessage = Resource.GetErrorMessage(MessageKeys.RESEARCHED_CONTENT_CANNOT_DELETE, base.GetLanguage(researchedContents), ex);
						throw new DataAccessException(errorMessage, ex, logCategory);
					} finally {
						context.Exit();
					}
				} else {
					this.DeleteResearchedContent((ResearchedContentDO)researchedContents[0]);
				}
			} else {
				string errorMessage = Resource.GetErrorMessage(MessageKeys.RESEARCHED_CONTENT_CANNOT_DELETE, base.GetLanguage(researchedContents));
				throw new DataAccessException(errorMessage, null, logCategory);
			}
		}

		/// <summary>
		/// Purpose: Delete a ResearchedContent record in the database
		/// </summary>
		/// <param name = "researchedContentDO">ResearchedContentDO researchedContentDO.</param>
		public override void DeleteResearchedContent(ResearchedContentDO researchedContentDO) {
			if (IsValid(this, MethodBase.GetCurrentMethod().Name, researchedContentDO)) {
				base.DeleteResearchedContent(researchedContentDO);
			}
		}

	}
}
