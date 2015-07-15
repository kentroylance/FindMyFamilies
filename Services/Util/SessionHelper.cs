using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using FindMyFamilies.Data;
using FindMyFamilies.Util;
using ProtoBuf;

namespace FindMyFamilies.Util {
    public class SessionHelper {
        private static readonly Logger logger = new Logger(MethodBase.GetCurrentMethod().DeclaringType);

        public static SessionDO GetSession(string personID) {
            return GetSession1(personID, false);
        }

        public static SessionDO GetSession1(string personID, bool resetExpiration) {
            string sessionFilePath = FilePathHelper.GetTempPath() + "/User" + personID + ".bin";
            logger.Error("sessionFilePath = " + sessionFilePath);

            SessionDO session = null;
            string errorMessage = null;
            if (File.Exists(sessionFilePath)) {
                using (var file = System.IO.File.OpenRead(sessionFilePath)) {
                    try {
                        session = Serializer.Deserialize<SessionDO>(file);

                        if ((session != null) && (!session.Authenticated || resetExpiration || session.Expired)) {
                            session.ResetExpiration();
                            SetSession1(session);
                        }
                        if (!string.IsNullOrEmpty(errorMessage)) {
                            File.SetAttributes(sessionFilePath, FileAttributes.Normal);
                            File.Delete(sessionFilePath);
                        }
                    } catch (Exception) {
                        errorMessage = "Unable to Deserialize session file";
                        logger.Error(errorMessage);
                        throw new Exception(errorMessage);
                    }
                }
            } else {
                errorMessage = "Unable to get session";
                logger.Error(errorMessage);
                throw new Exception(errorMessage);
            }
            return session;
        }

        public static void DeleteSession1(string personID) {
            logger.Error("DeleteSession");
            string sessionFilePath = FilePathHelper.GetTempPath() + "/User" + personID + ".bin";

            if (File.Exists(sessionFilePath)) {
                try {
                   File.SetAttributes(sessionFilePath, FileAttributes.Normal);
                   File.Delete(sessionFilePath);
                } catch (Exception e) {
                    logger.Error(e.Message, e);
                }
            }
        }


        public static void SetSession1(SessionDO session, bool resetExpiration) {
            string sessionFilePath = FilePathHelper.GetTempPath() + "/User" + session.CurrentPerson.Id + ".bin";
//            logger.Error("sessionFilePath = " + sessionFilePath);

            if (resetExpiration) {
                session.ResetExpiration();
            }
            using (var file = System.IO.File.Create(sessionFilePath)) {
                try {
                    Serializer.Serialize(file, session);
                    File.SetAttributes(sessionFilePath, FileAttributes.Normal);

                    if (File.Exists(sessionFilePath)) {
                        logger.Error("file exists sessionFilePath = " + sessionFilePath);
                    }

                } catch (Exception) {
                    string errorMessage = "Unable to serialize session file";
                    logger.Error(errorMessage);
                }
            }

        }

        public static void SetSession1(SessionDO session) {
            SetSession1(session, false);
        }
    }
}