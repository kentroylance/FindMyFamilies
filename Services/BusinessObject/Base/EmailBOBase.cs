///////////////////////////////////////////////////////////////////////////
// Description: Business Object Base class for the 'Email'
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
	/// Purpose: Business Object base class for Email
	/// </summary>
	public class EmailBOBase : BusinessObjectBase {
		// Constant Declarations
		private static readonly object logCategory = MethodBase.GetCurrentMethod().DeclaringType;
		private static EmailDAO m_EmailDAO;

		// Property Declarations
		public static EmailDAO EmailDAO {
			get {
				if (m_EmailDAO == null) {
					EmailBOBase emailBO = new EmailBOBase();
					lock(emailBO) {
						m_EmailDAO = new EmailDAO();
					}
				}
				return m_EmailDAO;
			}
		}

		public virtual EmailDO CreateEmail(EmailDO emailDO) {
			return EmailDAO.CreateEmail(emailDO);
		}

		public virtual EmailDO ReadEmail(EmailDO emailDO) {
			return EmailDAO.ReadEmail(emailDO);
		}

		public virtual ICollection ReadEmailsAll(DataTransferObject dataTransferObject) {
			return EmailDAO.ReadEmailsAll(dataTransferObject);
		}

		public virtual ICollection ReadEmailsAllList(DataTransferObject dataTransferObject) {
			return EmailDAO.ReadEmailsAllList(dataTransferObject);
		}

		public virtual void UpdateEmail(EmailDO emailDO) {
			EmailDAO.UpdateEmail(emailDO);
		}

		public virtual void DeleteEmail(EmailDO emailDO) {
			EmailDAO.DeleteEmail(emailDO);
		}

		public virtual ICollection ReadEmailsByPage(EmailDO emailDO) {
			return EmailDAO.ReadEmailsByPage(emailDO);
		}

	}
}