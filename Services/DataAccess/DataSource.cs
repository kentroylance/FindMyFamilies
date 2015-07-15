using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Runtime.Remoting.Messaging;
using FindMyFamilies.Exceptions;

namespace  FindMyFamilies.DataAccess
{
    /// <summary>
    /// DataSource is a facade class and represents a certain database.
    /// Produces connections and commands.
    /// Uses DataCommandFactory internally.
    /// </summary>
    class DataSource : IDataSource
    {
        private string _name = "";
        private DataProvider _provider = null;
        private string _connectionString = "";
        private DataOperationFactory _operationFactory = null;
        private IDbConnection _templateConnection = null;
        private IDbCommand _templateCommand = null;
        private IDbDataAdapter _templateDataAdapter = null;
        private string _parameterNamePrefix = "";

        public DataSource(
            string name, DataProvider provider, string connectionString, 
            string dataOperationsFileMask, string parameterNamePrefix)
        {
            _name = name;
            _provider = provider;
            _connectionString = connectionString;
            _operationFactory = new DataOperationFactory(this, dataOperationsFileMask);
            _templateConnection = (IDbConnection)Activator.CreateInstance(_provider.ConnectionObjectType);
            _templateCommand = (IDbCommand)Activator.CreateInstance(_provider.CommandObjectType);
            if(_provider.DataAdapterObjectType != null)
                _templateDataAdapter = (IDbDataAdapter)Activator.CreateInstance(_provider.DataAdapterObjectType);

            if(parameterNamePrefix != null)
                _parameterNamePrefix = provider.ParameterNamePrefix;
            else
                _parameterNamePrefix = provider.ParameterNamePrefix;

        }

        public string Name { get { return _name; } }
        public DataProvider Provider { get { return _provider; } }
        public string ParameterNamePrefix { get { return _parameterNamePrefix; } }

        public IDbConnection CreateConnection() 
        {
			IDbConnection dbCon = null;
			//dbCon = (IDbConnection)((ICloneable)_templateConnection).Clone();
			dbCon = (IDbConnection)Activator.CreateInstance(_provider.ConnectionObjectType);
			dbCon.ConnectionString = _connectionString;
        	return dbCon;
        }

        public IDataCommand GetCommand(string commandName) 
        {
            return _operationFactory.GetCommand(commandName);
        }

        public IDataCommand CreateCommand() 
        {
            IDbCommand dbCmd = (IDbCommand)Activator.CreateInstance(_provider.CommandObjectType);
            //IDbCommand dbCmd = (IDbCommand)((ICloneable)_templateCommand).Clone();
            IDataCommand cmd = new DataCommand(dbCmd, this);

            return cmd;
        }

        public IDataCommand CreateCommand(string commandText, CommandType commandType) 
        {
            //TODO: decide whether to delegate or do it here
            IDataCommand cmd = CreateCommand();

            cmd.DbCommand.CommandText = commandText;
            cmd.DbCommand.CommandType = commandType;

            return cmd;
        }

        public IDataSetAdapter GetDataSetAdapter(string adapterName) 
        {
            return _operationFactory.GetAdapter(adapterName);
        }
        public IDataSetAdapter CreateDataSetAdapter() 
        {
            if(_templateDataAdapter == null)
                throw new DataAccessException("DataSource does not support DataAdapters!");

            IDbDataAdapter dbDataAdapter = (IDbDataAdapter)((ICloneable)_templateDataAdapter).Clone();
            IDataSetAdapter dataSetAdapter = new DataSetAdapter(dbDataAdapter, this);

            return dataSetAdapter;
        }

        public IDataSetAdapter CreateDataSetAdapter(IDataCommand selectCommand) 
        {
            IDataSetAdapter dataSetAdapter = CreateDataSetAdapter();
            dataSetAdapter.SelectCommand = selectCommand;

            return dataSetAdapter;
        }
    }
}
