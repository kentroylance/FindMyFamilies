using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Collections;

using FindMyFamilies.Exceptions;
using FindMyFamilies.Util;

namespace  FindMyFamilies.DataAccess
{
    /// <summary>
    /// DataProviderFactory returns DataProvider objects.
    /// Maintains/refreshes internal cache of DataProviders.
    /// Config info retrieved from  FindMyFamilies.DataAccess.dll.config
    /// </summary>
    internal class DataProviderFactory
    {
        private static Hashtable _dataProviders = null;
        private const string DATAPROVIDERS_ROOT_ELEMENT = "dataProviders";

        //only static methods
        private DataProviderFactory() {}

        internal static DataProvider GetDataProvider(string dataProviderName)
        {
            if(_dataProviders == null) 
            {
                ConfigManager.ProcessConfig(
                    DATAPROVIDERS_ROOT_ELEMENT, new LoadXMLHandler(LoadDataProviders));
                
                if(_dataProviders == null) 
                    throw new DataAccessException("No DataProvider config data found.");
            }

            DataProvider dp = _dataProviders[dataProviderName] as DataProvider;

            if(dp == null) 
            {
                //TODO: specialize the exception, perhaps DataSourceNotFoundException
                throw new DataAccessException("DataProvider with name '" + dataProviderName + "' not found.");
            }

            return dp;
        }

        private static void LoadDataProviders(string xml) 
        {
            XmlTextReader xr = new XmlTextReader(new StringReader(xml));

            xr.WhitespaceHandling = WhitespaceHandling.None;
            xr.MoveToContent();

            Hashtable dataProviders = new Hashtable();
            string name = "";

            while(xr.Read())
            {
                switch(xr.NodeType) 
                {
                    case XmlNodeType.Element:
                        switch(xr.Name) 
                        {
                            case "dataProvider":
                                name = xr.GetAttribute("name");

                                if(name == null || name == string.Empty)
                                    throw new DataAccessException("No provider name specified.");

								string strConnectionType = xr.GetAttribute("connectionType");
								if(strConnectionType == null || strConnectionType == string.Empty)
									throw new DataAccessException("No connectionType specified for provider " + name);
								Type connectionType = Type.GetType(strConnectionType);
								if(connectionType == null)
									throw new DataAccessException("Could not load connectionType for typeName " + strConnectionType + " for provider " + name);

								string strCommandType = xr.GetAttribute("commandType");
								if(strCommandType == null || strCommandType == string.Empty)
									throw new DataAccessException("No commandType specified for provider " + name);
								Type commandType = Type.GetType(strCommandType);
								if(commandType == null)
									throw new DataAccessException("Could not load commandType for typeName " + strCommandType + " for provider " + name);

								string strParameterType = xr.GetAttribute("parameterType");
								if(strParameterType == null || strParameterType == string.Empty)
									throw new DataAccessException("No parameterType specified for provider " + name);
								Type parameterType = Type.GetType(strParameterType);
								if(parameterType == null)
									throw new DataAccessException("Could not load parameterType for typeName " + strParameterType + " for provider " + name);

								string strParameterDbTypeProperty = xr.GetAttribute("parameterDbTypeProperty");
								if(strParameterDbTypeProperty == null || strParameterDbTypeProperty == string.Empty)
									throw new DataAccessException("No parameterDbTypeProperty specified for provider " + name);
								PropertyInfo parameterDbTypeProperty = 
									parameterType.GetProperty(
									strParameterDbTypeProperty, 
									BindingFlags.Instance | BindingFlags.Public);
								if(parameterDbTypeProperty == null)
									throw new DataAccessException("Could not load parameterDbTypeProperty for typeName " + strParameterDbTypeProperty + " for provider " + name);

								string strParameterDbType = xr.GetAttribute("parameterDbType");
								if(strParameterDbType == null || strParameterDbType == string.Empty)
									throw new DataAccessException("No parameterDbType specified for provider " + name);
								Type parameterDbType = Type.GetType(strParameterDbType);
								if(parameterDbType == null)
									throw new DataAccessException("Could not load parameterDbType for typeName " + strParameterDbType + " for provider " + name);

								Type dataAdapterType = null;
								string strDataAdapterType = xr.GetAttribute("dataAdapterType");
								if(strDataAdapterType != null) {
									dataAdapterType = Type.GetType(strDataAdapterType);
									if(dataAdapterType == null)
										throw new DataAccessException("Could not load dataAdapterType for typeName " + strDataAdapterType + " for provider " + name);
								}

								Type commandBuilderType = null;
								string strCommandBuilderType = xr.GetAttribute("commandBuilderType");
								if(strCommandBuilderType != null) {
									commandBuilderType = Type.GetType(strCommandBuilderType);
									if(commandBuilderType == null)
										throw new DataAccessException("Could not load commandBuilderType for typeName " + strCommandBuilderType + " for provider " + name);
								}
                            
								string parameterNamePrefix = xr.GetAttribute("parameterNamePrefix");
								if(parameterNamePrefix == null)
									parameterNamePrefix = string.Empty;

								dataProviders.Add(name, 
									new DataProvider(
									name, connectionType, commandType, 
									parameterType, parameterDbType, parameterDbTypeProperty,
									dataAdapterType, commandBuilderType, parameterNamePrefix));
                        		
								break;
						}
                    break;
                }
            }
            xr.Close();

            _dataProviders = dataProviders;
        }
    }
}
