using System.Collections;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Xml;


namespace FindMyFamilies.Util {

	public delegate void LoadXMLHandler(string xml);

	/// <summary>
	/// ConfigManager maintains the configuration data read from xml files.
	/// Setups watchers to handle xml files' changes.
	/// Supplies xml to the subscribed parties, which parse it and instantiate their objects.
	/// </summary>
	public class ConfigManager {
		private class ConfigEntry {
			private Hashtable _xmlDocs = new Hashtable();
			private string _directoryPath = "";
			private string _fileMask = "";
			private Hashtable _elementHandlers = new Hashtable();

			public string DirectoryPath {
				get {
					return _directoryPath;
				}
				set {
					_directoryPath = value;
				}
			}

			public string FileMask {
				get {
					return _fileMask;
				}
				set {
					_fileMask = value;
				}
			}

			public Hashtable ElementHandlers {
				get {
					return _elementHandlers;
				}
				set {
					_elementHandlers = value;
				}
			}

			//private static DateTime _configLastChanged = DateTime.Now;

			public void ExecuteHandler(string rootElement, LoadXMLHandler xmlHandler) {
				XmlNode xmlNode = null;

				if (_xmlDocs.Count == 0) {
					return;
				}

				XmlDocument mergedDoc = new XmlDocument();
				xmlNode = mergedDoc.CreateElement("merged" + rootElement);
				mergedDoc.AppendChild(xmlNode);

				foreach (DictionaryEntry entry in _xmlDocs) {
					xmlNode = ((XmlDocument) entry.Value).SelectSingleNode("//" + rootElement);
					xmlNode = mergedDoc.ImportNode(xmlNode, true);
					mergedDoc.DocumentElement.AppendChild(xmlNode);
				}

				if (mergedDoc.ChildNodes[0].ChildNodes.Count > 0) {
					xmlHandler(mergedDoc.OuterXml);
				}
			}

			public void ExecuteHandlers() {
				foreach (DictionaryEntry entry in _elementHandlers) {
					if ((SystemInfo.Instance.ExecutingService || SystemInfo.Instance.ExecutingWebApp)) {
						ExecuteHandler((string) entry.Key, (LoadXMLHandler) entry.Value);
					}
				}
			}

			public void LoadXmlDocumentFromFiles() {
				string[] fileNames = Directory.GetFiles(_directoryPath, _fileMask);

				if (fileNames.Length == 0) {
					throw new System.Configuration.ConfigurationErrorsException("No config files found with mask " + _directoryPath + Path.DirectorySeparatorChar + _fileMask + ".");
				}

				if (_fileMask.IndexOf("Commands") > 0) {
					for (int i = 0; i < fileNames.Length; i++) {
						if ((fileNames[i].IndexOf("FindMyFamilies") > 0) && (fileNames[i].IndexOf("Base") > 0)) {
							LoadXmlDocumentFromFile(fileNames[i]);
						}
					}
					for (int i = 0; i < fileNames.Length; i++) {
						if ((fileNames[i].IndexOf("FindMyFamilies") > 0) && (fileNames[i].IndexOf("Base") < 1)) {
							LoadXmlDocumentFromFile(fileNames[i]);
						}
					}
					for (int i = 0; i < fileNames.Length; i++) {
						if ((fileNames[i].IndexOf("FindMyFamilies") < 1) && (fileNames[i].IndexOf("Base") > 0)) {
							LoadXmlDocumentFromFile(fileNames[i]);
						}
					}
					for (int i = 0; i < fileNames.Length; i++) {
						if ((fileNames[i].IndexOf("FindMyFamilies") < 1) && (fileNames[i].IndexOf("Base") < 1)) {
							LoadXmlDocumentFromFile(fileNames[i]);
						}
					}
				} else {
					for (int i = 0; i < fileNames.Length; i++) {
						LoadXmlDocumentFromFile(fileNames[i]);
					}
				}
				//				foreach (string fileName in fileNames) {
				//					LoadXmlDocumentFromFile(fileName);
				//				}
			}

			private void LoadXmlDocumentFromFile(string fileName) {
				fileName = fileName.ToLower(CultureInfo.InvariantCulture);

				XmlDocument doc = new XmlDocument();
				XmlTextReader reader = new XmlTextReader(fileName);
				reader.WhitespaceHandling = WhitespaceHandling.None;
				doc.Load(reader);
				reader.Close();
				if (_xmlDocs.Contains(fileName)) {
					_xmlDocs[fileName] = doc;
				} else {
					_xmlDocs.Add(fileName, doc);
				}
			}

			public void SetupWatcher() {
				FileSystemWatcher watcher = new FileSystemWatcher(DirectoryPath, FileMask);
				watcher.NotifyFilter = NotifyFilters.LastWrite;
				watcher.Changed += new FileSystemEventHandler(this.Config_Changed);
				watcher.EnableRaisingEvents = true;
				//_watchers.Add(...
			}

			private void Config_Changed(object sender, FileSystemEventArgs e) {
				/*
				//this is a hack to prevent the event from being double-raised
				TimeSpan timeDif = DateTime.Now - _configLastChanged;
				if(timeDif.Seconds > 2) 
				{
					//LoadDataSources(e.FullPath);
					_configLastChanged = DateTime.Now;
				}
*/
				//LoadXmlDocumentFromFile(e.FullPath);
				LoadXmlDocumentFromFile(e.FullPath);
				ExecuteHandlers();
			}
		}

		private static Hashtable _configEntries = new Hashtable();

		static ConfigManager() {
		}

		private ConfigManager() {
		}

		//gets the calling assembly directory and name
		public static void ProcessConfig(string rootElement, LoadXMLHandler xmlHandler) {
			string directoryName = string.Empty;
			string fileName = string.Empty;

			//string overrideFullPath = ConfigurationSettings.AppSettings[" FindMyFamilies.DataAccess." + rootElement];
			string overrideFullPath = SystemInfo.Instance.ApplicationName + ".DataAccess." + rootElement;
			directoryName = SystemInfo.Instance.ResourcePath;
			switch (rootElement) {
				case "dataSources":
					fileName = Path.GetFileName("FindMyFamilies.DataAccess.dataSources.config");
					break;
				case "dataProviders":
					fileName = Path.GetFileName("FindMyFamilies.DataAccess.dataSources.config");
					break;
				case "transactionHandlers":
					fileName = Path.GetFileName("FindMyFamilies.DataAccess.dataSources.config");
					break;
				default:
					fileName = Path.GetFileName(overrideFullPath);
					break;
			}
			ProcessConfig(fileName, rootElement, xmlHandler);
		}

		public static void ProcessConfig(string fileMask, string rootElement, LoadXMLHandler xmlHandler) {
			if (fileMask == null || fileMask == string.Empty) {
				string overrideFullPath = System.Configuration.ConfigurationManager.AppSettings[SystemInfo.Instance.ApplicationName + ".DataAccess." + rootElement];
				if (overrideFullPath != null) {
					if (fileMask == null || fileMask == string.Empty) {
						fileMask = Path.GetFileName(overrideFullPath);
					}
				} else {
					if (fileMask == null || fileMask == string.Empty) {
						fileMask = "*.*";
					}
				}
			}

			string fullPath = SystemInfo.Instance.ResourcePath + fileMask;

			ConfigEntry configEntry = _configEntries[fullPath] as ConfigEntry;
			if (configEntry == null) {
				configEntry = new ConfigEntry();
				_configEntries.Add(fullPath, configEntry);

				configEntry.DirectoryPath = SystemInfo.Instance.ResourcePath;
				configEntry.FileMask = fileMask;
				configEntry.LoadXmlDocumentFromFiles();
				//configEntry.SetupWatcher();
			}

			if (!configEntry.ElementHandlers.Contains(rootElement)) {
				configEntry.ElementHandlers.Add(rootElement, xmlHandler);
			}

			configEntry.ExecuteHandler(rootElement, xmlHandler);
		}
	}
}