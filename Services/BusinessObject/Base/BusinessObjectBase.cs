using System.Collections;
using System.Reflection;
using FindMyFamilies.Data;
using FindMyFamilies.DataAccess;

namespace FindMyFamilies.BusinessObject {
    /// <summary>
    ///     Summary description for BusinessObject base class.
    /// </summary>
    public class BusinessObjectBase {
        private static SecurityBO m_SecurityBO;
        private static readonly object logCategory = MethodBase.GetCurrentMethod().DeclaringType;

        public SecurityBO Security {
            get {
                if (m_SecurityBO == null) {
                    m_SecurityBO = new SecurityBO();
                }
                return m_SecurityBO;
            }
        }

        public string GetLanguage(IList dataTransferObjects) {
            return DataAccessObjectBase.GetLanguage(dataTransferObjects);
        }

        public bool IsValid(object businessObject, string methodName, DataTransferObject dataObject) {
            bool valid = true;
//			dataObject = Security.Authenticate(dataObject);
////			Logger.Error("IsValid.MemberID" + dataObject.MemberID, null, null, null);
//
//			string className = businessObject.GetType().Name;
//
//			if (!methodName.Equals("ReadMemberByLoginID")) {
//				ValidationDO validation = ValidationManager.Instance().GetValidation(className, methodName, dataObject.Language);
//
//				for (int i = 0; i < validation.Fields.Count; i++) {
//					ValidationFieldDO field = (ValidationFieldDO) validation.Fields[i];
//					object fieldValue = Reflection.GetProperty(dataObject, field.Name);
//					//object fieldValue = dataTransferObject.GetType().InvokeMember(field.name, BindingFlags.GetProperty, null, dataTransferObject, null);
//					string errorMessage = null;
//
//					if (fieldValue != null) {
//						if (fieldValue is string) {
//							string stringValue = (string) fieldValue;
//
//							if (Strings.IsEmpty(stringValue)) {
//								valid = false;
//							} else if (stringValue.Trim().Length > field.Length) {
//								valid = false;
//								errorMessage = Resource.GetErrorMessage(MessageKeys.INFO_EXCEEDS_FIELD_LENGTH, dataObject.Language) + "; Field: " + field.Name + "; Value: " + fieldValue;
//							}
//						} else if (fieldValue is int) {
//							if ((int) fieldValue < 1) {
//								valid = false;
//							}
//						} else if (fieldValue is float) {
//							if ((float) fieldValue < 1) {
//								valid = false;
//							}
//						} else if (fieldValue is DateTime) {
//							if (((DateTime) fieldValue).ToShortDateString().Equals("1/1/0001")) {
//								valid = false;
//							}
//						}
//					} else {
//						valid = false;
//					}
//
//					if (!valid) {
//						if (Strings.IsEmpty(errorMessage)) {
//							errorMessage = Resource.GetErrorMessage(MessageKeys.INFO_EMPTY_REQUIRED_FIELD, dataObject.Language) + "; Field: " + field.Name + "; Value: " + fieldValue;
//						}
//						new ValidationException(errorMessage, null, logCategory);
//					}
//				}
//			}
////			Logger.Error("IsValid.MemberID" + dataObject.MemberID, null, null, null);
            return valid;
        }
    }
}