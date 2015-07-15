using System.Reflection;

namespace FindMyFamilies.BusinessObject {
    /// <summary>
    ///     Summary description for SecurityBO.
    /// </summary>
    public class SecurityBO : BusinessObjectBase {
        public const string SECURITY = "Security";

        private static readonly object logCategory = MethodBase.GetCurrentMethod().DeclaringType;

//        public DataTransferObject GetSecurityData(string loginID) {
//            var security = (SecurityDO) CacheManager.Get(SECURITY + loginID);
//            return new DataTransferObject(loginID, security.LoginPassword, security.Member.MemberID, security.Member.LanguageID);
//        }
//
//        public void VerifyVersions(ref SecurityDO security, ref DataTransferObject dataTransferObject, bool securityCached) {
//            bool downloadGroups = false;
//
//            if (dataTransferObject.LoggingIn || dataTransferObject.Verify) {
//                if (dataTransferObject.RemoteClient) {
//                    if (dataTransferObject.Verify) {
//                        downloadGroups = true;
//                    }
//                    var localVersions = (Hashtable) dataTransferObject.Data[DataTransferObject.LOCAL_VERSIONS];
//                    var serverVersions = (Hashtable) new VersionDAO().ReadVersionsAll(dataTransferObject);
//
//                    bool downloadMenus = false;
//                    bool downloadTranslations = false;
//
//                    dataTransferObject.NextIDs = (IDictionary) new NextIDDAO().ReadNextIDsAll(dataTransferObject);
//                    dataTransferObject.ServerVersions = serverVersions;
//                }
//                if (security.Member.SecurityChanged || downloadGroups || !securityCached) {
//                    security.Groups = (IDictionary) new GroupDAO().ReadGroupAll(dataTransferObject);
//                    security.Member_Groups = (IList) new GroupDAO().ReadGroupsByMemberID(dataTransferObject);
//
//                    // it looks like ReadMemberRightsByGroup and ReadRightsByMember are returning the same thing.
//                    security.Rights = (IDictionary) new SecurityAdminBO().ReadMemberRightsByGroup(security);
//
//                    var securityAdminDO = new SecurityAdminDO();
//                    securityAdminDO.MemberID = security.LoginMemberID;
//                    securityAdminDO.GetSecurityData(dataTransferObject);
//                    var memberRights = (Hashtable) new SecurityAdminBO().ReadRightsByMember(securityAdminDO);
//                    security.AddToRights(memberRights);
//
//                    if (security.Member.SecurityChanged) {
//                        security.Member.SecurityChanged = false;
//                        new MemberDAO().UpdateMember(security.Member);
//                    }
//                }
//            }
//        }
//
//        public DataTransferObject Authenticate(DataTransferObject dataTransferObject) {
//            if (dataTransferObject != null) {
//                if (Strings.IsEmpty(dataTransferObject.LoginID) && Strings.IsEmpty(dataTransferObject.LoginPassword)) {
//                    string errorMessage = Resource.GetErrorMessage(MessageKeys.INFO_EMPTY_SECURITY, dataTransferObject.Language);
//                    throw new SecurityException(errorMessage, null, logCategory);
//                } else if (Strings.IsEmpty(dataTransferObject.LoginID)) {
//                    string errorMessage = Resource.GetErrorMessage(MessageKeys.INFO_EMPTY_USER_NAME, dataTransferObject.Language);
//                    throw new SecurityException(errorMessage, null, logCategory);
//                } else if (Strings.IsEmpty(dataTransferObject.LoginPassword)) {
//                    string errorMessage = Resource.GetErrorMessage(MessageKeys.INFO_EMPTY_USER_NAME, dataTransferObject.Language);
//                    throw new SecurityException(errorMessage, null, logCategory);
//                } else {
//                    string key = null;
//                    key = SECURITY + dataTransferObject.LoginID;
//
//                    var security = (SecurityDO) CacheManager.Get(key);
//                    if (security != null) {
//                        if ((!security.LoginPassword.Equals(dataTransferObject.LoginPassword)) ||
//                            (!security.LoginPassword.Equals(dataTransferObject.LoginPassword))) {
//                            string errorMessage = Resource.GetErrorMessage(MessageKeys.INFO_CANNOT_LOGIN, dataTransferObject.Language) + ". Key: " +
//                                key.ToString();
//                            new SecurityException(errorMessage, null, logCategory);
//                        } else {
//                            if (!SystemInfo.Instance.ExecutingWebApp) {
//                                VerifyVersions(ref security, ref dataTransferObject, true);
//                                if (dataTransferObject.Data[DataTransferObject.SECURITY] == null) {
//                                    dataTransferObject.Data.Add(DataTransferObject.SECURITY, security);
//                                } else {
//                                    dataTransferObject.Data[DataTransferObject.SECURITY] = security;
//                                }
//                            }
//                        }
//                    } else {
//                        var member = new MemberDO();
//                        var memberDAO = new MemberDAO();
//                        member.MemberLoginID = dataTransferObject.LoginID;
//                        member = memberDAO.ReadMemberByLoginID(member);
//                        if (member != null) {
//                            if ((dataTransferObject != null) && (member.MemberPassword.Equals(dataTransferObject.LoginPassword))) {
//                                //if (!response.HasErrors() && (security != null) && (userDO.userID > 0) && (security.password == userDO.password)) {
//                                // need to add code for getting usergroups after the generator works
//                                dataTransferObject.LoginMemberID = member.MemberID;
//                                security = new SecurityDO(dataTransferObject.LoginID, dataTransferObject.LoginPassword, member.MemberID);
//                                security.Member = member;
//                                if (!SystemInfo.Instance.ExecutingWebApp) {
//                                    VerifyVersions(ref security, ref dataTransferObject, false);
//                                }
//                                CacheManager.Set(key, security, CacheManager.INTERVAL_MINUTES, 20);
//                                dataTransferObject.Security = security;
//                            } else {
//                                string errorMessage = Resource.GetErrorMessage(MessageKeys.INFO_CANNOT_LOGIN, dataTransferObject.Language);
//                                throw new SecurityException(errorMessage, null, logCategory);
//                            }
//                        }
//                    }
//                }
//            } else {
//                string errorMessage = Resource.GetErrorMessage(MessageKeys.INFO_EMPTY_SECURITY, Constants.LANGUAGE_DEFAULT);
//                throw new SecurityException(errorMessage, null, logCategory);
//            }
//            return dataTransferObject;
//        }
    }
}