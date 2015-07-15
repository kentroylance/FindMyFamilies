using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Xml;

using FindMyFamilies.Exceptions;
using FindMyFamilies.Util;

namespace  FindMyFamilies.DataAccess {
	/// <summary>
	/// Each DataSource has one DataOperationFactory and delegates work to it.
	/// Returns DataCommands/DataSetAdapters from the cache.
	/// Maintains/refreshes internal DataCommand/DataSetAdapters caches.
	/// Config info retrieved from dataOperationsDir/dataOperationsFileMask from the DataSource.
	/// </summary>
	class DataOperationFactory {
		private DataSource _dataSource = null;
		private DataProvider _provider = null;

		private string _dataOperationsFileMask = "";

		private const string DATAOPERATIONS_ROOT_ELEMENT = "dataOperations";

		private Hashtable _commands = null;
		private Hashtable _commandtypes = null;
		private Hashtable _adapters = null;

		public DataOperationFactory(DataSource dataSource, string dataOperationsFileMask) {
			_dataSource = dataSource;
			_provider = dataSource.Provider;
			_dataOperationsFileMask = dataOperationsFileMask;
		}

		public IDataCommand GetCommand(string commandName) {
			if (_commands == null) {
				ConfigManager.ProcessConfig(_dataOperationsFileMask, DATAOPERATIONS_ROOT_ELEMENT, new LoadXMLHandler(LoadDataOperations));
				if (_commands == null) {
					throw new DataAccessException("No DataCommand config data found for dataSource with name " + _dataSource.Name);
				}
			}

			DataCommand cmd = _commands[commandName] as DataCommand;
			if (cmd == null) {
				throw new DataAccessException("DataCommand with name " + commandName + " not found.");
			}

			// IDataCommand newCmd = (IDataCommand)cmd.Clone();

			return cmd;
		}

		public IDataSetAdapter GetAdapter(string adapterName) {
			if (_adapters == null) {
				ConfigManager.ProcessConfig(_dataOperationsFileMask, DATAOPERATIONS_ROOT_ELEMENT, new LoadXMLHandler(LoadDataOperations));

				if (_adapters == null) {
					throw new DataAccessException("No DataSetAdapter config data found for DataSource with name " + _dataSource.Name);
				}
			}

			DataSetAdapter adapter = _adapters[adapterName] as DataSetAdapter;
			if (adapter == null) {
				throw new DataAccessException("DataSetAdapter with name " + adapterName + " not found.");
			}

			DataSetAdapter newAdapter = (DataSetAdapter) adapter.Clone();

			return newAdapter;
		}

		private void LoadDataOperations(string xml) {
			Hashtable commands = new Hashtable();
			Hashtable adapters = new Hashtable();

			XmlTextReader xr = new XmlTextReader(new StringReader(xml));

			xr.WhitespaceHandling = WhitespaceHandling.None;
			xr.MoveToContent();

			DataCommand currentDataCommand = null;
			IDbCommand currentDbCommand = null;
			bool isCommandTextCurrentNode = false;
			DataSetAdapter currentDataSetAdapter = null;
			IDbDataAdapter currentDbDataAdapter = null;
			bool populateCommands = false;

			while (xr.Read()) {
				switch (xr.NodeType) {
					case XmlNodeType.Element:
						switch (xr.Name) {
							case "dataCommand":
								//instantiate the command
								string commandName = xr.GetAttribute("name");

								try {
									currentDbCommand =
										(IDbCommand) Activator.CreateInstance(_provider.CommandObjectType);

									string strCommandType = xr.GetAttribute("type");
									CommandType commandType =
										(CommandType) Enum.Parse(typeof (CommandType), strCommandType, true);
									currentDbCommand.CommandType = commandType;

									currentDataCommand = new DataCommand(commandName, currentDbCommand, _dataSource);
								} catch (Exception e) {
									Console.Write(e);
								}
								if (!commands.ContainsKey(commandName)) {
									commands.Add(commandName, currentDataCommand);
								} else {
									string test = "duplicate";
								}
								break;

							case "commandText":
								isCommandTextCurrentNode = true;
								break;

							case "param":
								string paramKey = xr.GetAttribute("key");
								if (paramKey == null) {
									paramKey = string.Empty;
								}
								string paramName = xr.GetAttribute("name");
								string strParamType = xr.GetAttribute("type");
								Enum paramType =
									(Enum) Enum.Parse(_provider.ParameterDbType, strParamType, true);
								string strSize = xr.GetAttribute("size");
								int size = 0;
								if (strSize != null) {
									size = Int32.Parse(strSize);
								}
								ParameterDirection paramDirection =
									(ParameterDirection) Enum.Parse(typeof (ParameterDirection), xr.GetAttribute("direction"));
								string strSourceColumn = xr.GetAttribute("sourceColumn");
								DataRowVersion sourceVersion = DataRowVersion.Default;
								string strSourceVersion = xr.GetAttribute("sourceVersion");
								if (strSourceVersion != null) {
									sourceVersion = (DataRowVersion) Enum.Parse(typeof (DataRowVersion), strSourceVersion, true);
								}

								currentDataCommand.Parameters.Add(paramKey, paramName,
								                                  paramType, size, paramDirection, strSourceColumn, sourceVersion);
								/*
                            IDbDataParameter param = currentDbCommand.CreateParameter();
                            param.ParameterName = xr.GetAttribute("name");
                            string strParamType = xr.GetAttribute("type");
                            _provider.ParameterDbTypeProperty.SetValue(
                                param, Enum.Parse(_provider.ParameterDbType, strParamType, true), null);
                            string size = xr.GetAttribute("size");
                            if(size != null)
                                param.Size = Int32.Parse(xr.GetAttribute("size"));
                            param.Direction = 
                                (ParameterDirection)Enum.Parse(typeof(ParameterDirection), xr.GetAttribute("direction"));
                            string sourceColumn = xr.GetAttribute("sourceColumn");
                            if(sourceColumn != null) 
                                param.SourceColumn = sourceColumn;
                            string sourceVersion = xr.GetAttribute("sourceVersion");
                            if(sourceVersion != null) 
                                param.SourceVersion = 
                                    (DataRowVersion)Enum.Parse(typeof(DataRowVersion), sourceVersion, true);
                            currentDbCommand.Parameters.Add(param);
*/
								break;

							case "dataSetAdapter":
								//instantiate the adapter
								string adapterName = xr.GetAttribute("name");

								currentDbDataAdapter =
									(IDbDataAdapter) Activator.CreateInstance(_provider.DataAdapterObjectType);
								string strPopulateCommands = xr.GetAttribute("populateCommands");
								if (strPopulateCommands != null) {
									populateCommands = bool.Parse(strPopulateCommands);
								}
								currentDataSetAdapter =
									new DataSetAdapter(adapterName, currentDbDataAdapter, _dataSource);
								break;

							case "tableMapping":
								currentDataSetAdapter.TableMappings.Add(
									xr.GetAttribute("sourceTable"),
									xr.GetAttribute("dataSetTable"));
								break;
						}
						break;

					case XmlNodeType.Text:
						if (isCommandTextCurrentNode) {
							currentDbCommand.CommandText = xr.Value;
						}
						break;

					case XmlNodeType.CDATA:
						if (isCommandTextCurrentNode) {
							currentDbCommand.CommandText = xr.Value;
						}
						break;

					case XmlNodeType.EndElement:
						switch (xr.Name) {
							case "dataCommand":
								if (currentDataSetAdapter == null) {
									if (!commands.ContainsKey(currentDataCommand.Name)) {
										commands.Add(currentDataCommand.Name, currentDataCommand);
										currentDataCommand = null;
									}
								}
								break;

							case "commandText":
								isCommandTextCurrentNode = false;
								break;
							case "dataSetAdapter":
								if (populateCommands) {
									currentDataSetAdapter.PopulateCommands();
									populateCommands = false;
								}
								adapters.Add(currentDataSetAdapter.Name, currentDataSetAdapter);
								currentDataSetAdapter = null;
								break;

							case "selectCommand":
								currentDataSetAdapter.SelectCommand = currentDataCommand;
								currentDataCommand = null;
								break;
							case "deleteCommand":
								currentDataSetAdapter.DeleteCommand = currentDataCommand;
								currentDataCommand = null;
								break;
							case "insertCommand":
								currentDataSetAdapter.InsertCommand = currentDataCommand;
								currentDataCommand = null;
								break;
							case "updateCommand":
								currentDataSetAdapter.UpdateCommand = currentDataCommand;
								currentDataCommand = null;
								break;

						}
						break;
				}
			}
			xr.Close();

			_commands = commands;
			_adapters = adapters;
		}

	}
}