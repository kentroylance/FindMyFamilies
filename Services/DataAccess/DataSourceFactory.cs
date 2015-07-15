using System;
using System.Collections;
using System.IO;
using System.Xml;
using FindMyFamilies.Exceptions;
using FindMyFamilies.Util;

namespace  FindMyFamilies.DataAccess {
    /// <summary>
    ///     DataSourceFactory returns DataSource objects.
    ///     Maintains/refreshes internal cache of DataSources.
    ///     Config info retrieved from  FindMyFamilies.DataAccess.dll.config
    /// </summary>
    public class DataSourceFactory {
        private const string DEFAULT_DATASOURCE_NAME = "DEFAULT_DATASOURCE_NAME";
        private const string DATASOURCES_ROOT_ELEMENT = "dataSources";

        private static Hashtable _dataSources;
        private static IDataSource _defaultDataSource;
        private static string _defaultAccountingDataSource = "";

        //only static methods
        private DataSourceFactory() {
        }

        public static string DefaultAccountingDataSource {
            get {
                return _defaultAccountingDataSource;
            }
        }

        public static IDataSource GetDataSource() {
            return GetDataSource(DEFAULT_DATASOURCE_NAME);
        }

        public static IDataSource GetDataSource(string dataSourceName) {
            if (_dataSources == null) {
                ConfigManager.ProcessConfig(DATASOURCES_ROOT_ELEMENT, LoadDataSources);

                if (_dataSources == null) {
                    throw new DataAccessException("No DataSource config data found.");
                }
            }

            IDataSource ds = null;

            if (dataSourceName.Equals(DEFAULT_DATASOURCE_NAME)) {
                ds = _defaultDataSource;
            } else {
                ds = _dataSources[dataSourceName] as DataSource;
            }

            if (ds == null) {
                ds = _dataSources["FindMyFamilies"] as DataSource; // need to design a better solution
                //TODO: specialize the exception, perhaps DataSourceNotFoundException
                //throw new DataAccessException("DataSource with name '" + dataSourceName + "' not found.");
            }

            return ds;
        }

        private static void LoadDataSources(string xml) {
            var xr = new XmlTextReader(new StringReader(xml));

            xr.WhitespaceHandling = WhitespaceHandling.None;
            xr.MoveToContent();

            var dataProviders = new Hashtable();
            var dataSources = new Hashtable();
            string name = "";

            while (xr.Read()) {
                switch (xr.NodeType) {
                    case XmlNodeType.Element:
                        switch (xr.Name) {
                            case "dataSource":
                                name = xr.GetAttribute("name");
                                if (name == null || name == string.Empty) {
                                    throw new DataAccessException("No dataSource name specified.");
                                }
                                string connectionString = xr.GetAttribute("connectionString");
                                if (connectionString == null || connectionString == string.Empty) {
                                    throw new DataAccessException("No connectionString specified for dataSource " + name);
                                }

                                string dataOperationsFileMask = xr.GetAttribute("dataOperationsFileMask");
                                if (dataOperationsFileMask == String.Empty) {
                                    dataOperationsFileMask = "*.*";
                                }

                                string providerName = xr.GetAttribute("provider");
                                if (providerName == null || providerName == string.Empty) {
                                    throw new DataAccessException("No provider specified for dataSource " + name);
                                }
                                DataProvider provider = DataProviderFactory.GetDataProvider(providerName);

                                string parameterNamePrefix = xr.GetAttribute("parameterNamePrefix");
                                if (parameterNamePrefix == null) {
                                    parameterNamePrefix = string.Empty;
                                }

                                var ds = new DataSource(name, provider, connectionString, dataOperationsFileMask, parameterNamePrefix);
                                dataSources.Add(ds.Name, ds);

                                string strDefault = xr.GetAttribute("default");
                                if (strDefault != null) {
                                    bool isDefault = bool.Parse(strDefault);
                                    if (isDefault) {
                                        _defaultDataSource = ds;
                                    }
                                }

                                // code added by shahzad as on May 09,2005
                                string strDefaultAccountingSystem = xr.GetAttribute("defaultAccountingSystem");
                                if (strDefaultAccountingSystem != null) {
                                    bool isDefault = bool.Parse(strDefaultAccountingSystem);
                                    if (isDefault) {
                                        _defaultAccountingDataSource = ds.Name;
                                    }
                                }
                                //end code addition 

                                break;
                        }
                        break;
                }
            }
            xr.Close();

            _dataSources = dataSources;
        }
    }
}