///////////////////////////////////////////////////////////////////////////
// Description: Business Object Base class for the 'Next_ID'
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
	/// Purpose: Business Object base class for NextID
	/// </summary>
	public class NextIDBOBase : BusinessObjectBase {
		// Constant Declarations
		private static readonly object logCategory = MethodBase.GetCurrentMethod().DeclaringType;
		private static NextIDDAO m_NextIDDAO;

		// Property Declarations
		public static NextIDDAO NextIDDAO {
			get {
				if (m_NextIDDAO == null) {
					NextIDBOBase nextIDBO = new NextIDBOBase();
					lock(nextIDBO) {
						m_NextIDDAO = new NextIDDAO();
					}
				}
				return m_NextIDDAO;
			}
		}

		public virtual NextIDDO CreateNextID(NextIDDO nextIDDO) {
			return NextIDDAO.CreateNextID(nextIDDO);
		}

		public virtual NextIDDO ReadNextID(NextIDDO nextIDDO) {
			return NextIDDAO.ReadNextID(nextIDDO);
		}

		public virtual ICollection ReadNextIDsAll(DataTransferObject dataTransferObject) {
			return NextIDDAO.ReadNextIDsAll(dataTransferObject);
		}

		public virtual void UpdateNextID(NextIDDO nextIDDO) {
			NextIDDAO.UpdateNextID(nextIDDO);
		}

		public virtual void DeleteNextID(NextIDDO nextIDDO) {
			NextIDDAO.DeleteNextID(nextIDDO);
		}

		public virtual ICollection ReadNextIDsByPage(NextIDDO nextIDDO) {
			return NextIDDAO.ReadNextIDsByPage(nextIDDO);
		}

	}
}
