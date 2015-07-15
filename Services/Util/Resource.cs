using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Xml;
using FindMyFamilies.Data;
using FindMyFamilies.DataAccess;
using FindMyFamilies.Exceptions;

namespace FindMyFamilies.Util {
    /// <summary>
    ///     Summary description for Resource.
    /// </summary>
    public class Resource {
        private static readonly Logger logger = new Logger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly object logCategory = MethodBase.GetCurrentMethod().DeclaringType;
        private static HybridDictionary m_Resources;

        public static HybridDictionary Resources {
            get {
                if (m_Resources == null) {
                    m_Resources = new HybridDictionary();
                }
                return m_Resources;
            }
            set {
                Resources = value;
            }
        }

        public static string GetString(string key, string resourceName, string language) {
            if (Strings.IsEmpty(key)) {
                string errorMessage = "The <key> parameter ResourceServer.GetString is empty or null";
                throw new ResourceException(errorMessage, null, logCategory);
            }
            if (Strings.IsEmpty(resourceName)) {
                string errorMessage = "The <resourceName> parameter for ResourceServer.GetString is empty or null";
                throw new ResourceException(errorMessage, null, logCategory);
            }
            if (Strings.IsEmpty(language)) {
                string errorMessage = "The <language> parameter for ResourceServer.GetString is empty or null";
                throw new ResourceException(errorMessage, null, logCategory);
            }
            String text = "";
            try {
                var resourceManager = (ResourceManager) Resources["resourceServer" + resourceName];
                if (resourceManager == null) {
                    string resourcePath = SystemInfo.Instance.ResourcePath + "Resources\\";
                    string resourceFile = resourceName + "Server." + language + ".resources";
                    if (!File.Exists(resourcePath + resourceFile)) {
                        LoadResource(resourceName, language);
                    }

                    resourceManager = ResourceManager.CreateFileBasedResourceManager(resourceName, resourcePath, null);
                    Resources.Add("resourceServer" + resourceName, resourceManager);
                }
                if (resourceManager != null) {
                    text = resourceManager.GetString(key, Thread.CurrentThread.CurrentUICulture);
                }
            } catch (Exception ex) {
                string errorMessage = "Error while getting resource for string: " + key;
                throw new ResourceException(errorMessage, ex, logCategory);
            }
            return text;
        }

        public static string GetErrorMessage(string key, string language, Exception ex) {
            String msgText = "";
            try {
                msgText = GetErrorMessage(key, language);
            } catch (Exception e) {
                throw e;
            }
            return msgText;
        }

        public static string GetErrorMessage(string key, string language) {
            String text = key;

//            if (Strings.IsEmpty(key)) {
//                string errorMessage = "The <key> parameter ResourceServer.GetString is empty or null";
//                throw new ResourceException(errorMessage, null, logCategory);
//            }
//            if (Strings.IsEmpty(language)) {
//                string errorMessage = "The <language> parameter for GetString is empty or null";
//                throw new ResourceException(errorMessage, null, logCategory);
//            }
//            //Code block Added By Shahzad
//            //Only for development purposes need to be removed for production purpose
//            // This code block make sure that in development we always pick error messages
//            // from Messages and MessagesBase Xml files and not from database.
//            if (SystemInfo.Instance.Development) {
//                String mText = "";
//                mText = GetErrorMessage(key, false);
//                if (mText.Equals("")) {
//                    mText = GetErrorMessage(key, true);
//                }
//                return mText;
//            }
//            //End Addition
//            String text = "";
//            try {
//                var resourceManager = (ResourceManager) Resources["resourceServerErrorMessages"];
//                if (resourceManager == null) {
//                    string resourcePath = SystemInfo.Instance.ResourcePath + "\\";
//                    //string resourceFile = "ErrorMessagesServer." + language + ".resources"; 
//                    //if (!File.Exists(resourcePath + resourceFile)) {
//                    LoadResource("ErrorMessages", language);
//                    //}
//
//                    resourceManager = ResourceManager.CreateFileBasedResourceManager("ErrorMessages", resourcePath, null);
//                    Resources.Add("resourceServerErrorMessages", resourceManager);
//                }
//                if (resourceManager != null) {
//                    text = resourceManager.GetString(key, new CultureInfo(language));
//                }
//            } catch (Exception ex) {
//                string errorMessage = "Error while getting resource for string: " + key;
//                throw new ResourceException(errorMessage, ex, logCategory);
//            }
            return text;
        }

        public static string GetErrorMessage(string key, bool isBase) {
            string messageText = "";
            try {
                string appName = SystemInfo.Instance.ApplicationName;
                string path = Assembly.GetExecutingAssembly().Location;
                string name = Path.GetFileName(path);
                path = path.Replace(name, "");

                string filePath = "";
                if (isBase) {
                    filePath = path + "MessagesBase." + appName + ".xml";
                } else {
                    filePath = path + "Messages." + appName + ".xml";
                }

                using (FileStream fileSteam = File.OpenRead(filePath)) {
                    XmlReaderSettings settings;

                    settings = new XmlReaderSettings();
                    settings.ConformanceLevel = ConformanceLevel.Document;

                    using (XmlReader reader = XmlReader.Create(fileSteam, settings)) {
                        while (reader.Read()) {
                            if (reader.NodeType == XmlNodeType.Element) {
                                if (reader.Name.Equals("message")) {
                                    if (reader.GetAttribute("key") == key) {
                                        messageText = reader.GetAttribute("value");
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

            } catch (Exception ex) {
                string errorMessage = "Error while getting message for key: " + key;
                logger.Error(errorMessage, ex);
                throw new Exception(errorMessage, ex);
            }
            return messageText;
        }

        //End Addition by shahzad 

        public static ListItemDO GetListItem(string listName, string key, string language) {
            ListItemDO listItem = null;
            IList list = GetList(listName, language);
            int index = list.IndexOf(key);
            if (index > 0) {
                listItem = (ListItemDO) list[index];
            }
            return listItem;
        }

        public static IList GetList(string listName, string language) {
            if (Strings.IsEmpty(listName)) {
                //WindowsUtil.DisplayError("List name: " + listName + " is empty", null, logCategory);
                return new ArrayList();
            }
            IList list = null;
            try {
                list = (IList) Resources[listName];
                if (list == null) {
                    string resourcePath = SystemInfo.Instance.ResourcePath + "Resources\\";
                    //string resourceFile = listName + "." + language + ".resources"; 
                    //if (!File.Exists(resourcePath + resourceFile)) {
                    LoadResource(listName, language);
                    //}

                    ResourceManager resourceManager = ResourceManager.CreateFileBasedResourceManager(listName, resourcePath, null);
                    list = new ArrayList();
                    ResourceSet resourceSet = resourceManager.GetResourceSet(new CultureInfo(language), false, true);
                    IDictionaryEnumerator en = resourceSet.GetEnumerator();
                    ListItemDO listItem = null;
                    while (en.MoveNext()) {
                        listItem = new ListItemDO((string)en.Key, (string)en.Value);
                        list.Add(listItem);
                    }
                    Resources.Add(listName, list);
                }
            } catch (Exception ex) {
                string errorMessage = "Error while getting list: " + listName;
                throw new ResourceException(errorMessage, ex, logCategory);
            }
            return list;
        }

        public static void LoadResource(string resourceName, string language) {
            var translationMasterDO = new TranslationMasterDO();
            translationMasterDO.TranslationMasterName = resourceName;
            translationMasterDO.Language = language;
            var translationsForResource = (IList) new TranslationMasterDAO().ReadTranslationMastersByNameLanguage(translationMasterDO);
            // Retrieving master list from the database
            var translations = (string[,]) translationsForResource[0];
            string resourcePath = SystemInfo.Instance.ResourcePath + "Resources\\";
            Directory.CreateDirectory(resourcePath);
            var resourceWriter = new ResourceWriter(resourcePath + resourceName + "." + language + ".resources");
            for (int i = 0; i < translations.Length/2; i++) {
                resourceWriter.AddResource(translations[i, 0], translations[i, 1]);
            }
            resourceWriter.Generate();
            resourceWriter.Close();
//			string test = Resource.GetText("firstnameLabel", "Member");
//			string test1 = Resource.GetText("firstnameLabel", "Member");
        }
    }
}