using System;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Diagnostics;

namespace  FindMyFamilies.DataAccess
{
    /// <summary>
    /// Summary description for DataSetAdapter.
    /// </summary>
    class DataSetAdapter : IDataSetAdapter
    {
        private DataSource _dataSource = null;
        private string _name = "";

        private IDbDataAdapter _dbDataAdapter = null;
        private object _commandBuilder = null;

        private IDataCommand _selectCommand = null;
        private IDataCommand _updateCommand = null;
        private IDataCommand _insertCommand = null;
        private IDataCommand _deleteCommand = null;

        public DataSetAdapter(string adapterName, DataSource dataSource)
        {
            _name = adapterName;
            _dataSource = dataSource;
        }

        public DataSetAdapter(IDbDataAdapter dbDataAdapter, DataSource dataSource) 
        {
            _dbDataAdapter = dbDataAdapter;
            _dataSource = dataSource;
        }

        public DataSetAdapter(string adapterName, IDbDataAdapter dbDataAdapter, DataSource dataSource) 
            : this(adapterName, dataSource)
        {
            _dbDataAdapter = dbDataAdapter;
        }

        public DataSetAdapter(
            string adapterName, 
            IDbDataAdapter dbDataAdapter,
            DataSource dataSource,
            IDataCommand selectCommand,
            IDataCommand updateCommand,
            IDataCommand insertCommand,
            IDataCommand deleteCommand) : this(adapterName, dataSource)
        {
            _dbDataAdapter = dbDataAdapter;
            _selectCommand = selectCommand;
            _updateCommand = updateCommand;
            _insertCommand = insertCommand;
            _deleteCommand = deleteCommand;
        }

        public object Clone() 
        {
            DataSetAdapter newDataSetAdapter = new DataSetAdapter(
                _name, 
                (IDbDataAdapter)((ICloneable)_dbDataAdapter).Clone(), 
                _dataSource);

            newDataSetAdapter.SelectCommand = (IDataCommand)_selectCommand.Clone();

            if(_updateCommand != null)
                newDataSetAdapter.UpdateCommand = (IDataCommand)_updateCommand.Clone();
            if(_insertCommand != null)
                newDataSetAdapter.InsertCommand = (IDataCommand)_insertCommand.Clone();
            if(_deleteCommand != null)
                newDataSetAdapter.DeleteCommand = (IDataCommand)_deleteCommand.Clone();

            return newDataSetAdapter;
        }

        public string Name { get { return _name; } set { _name = value; } }
        public IDataSource DataSource { get { return _dataSource; } }

        public int Fill(DataSet ds) 
        {
            if(_selectCommand == null)
                throw new DataException("Select command should be defined before trying to Fill a DataSet using IDataSetAdapter.");

            _dbDataAdapter.SelectCommand = _selectCommand.DbCommand;

            DataSession dataSession = DataSessionFactory.GetDataSession(_dataSource);

            IDbConnection con = null;
            IDbTransaction tran = null;
            int recordsAffected = 0;

            if(dataSession != null) 
            {
                con = dataSession.Connection;
                tran = dataSession.Transaction;
            }
            else 
            {
                con = _dataSource.CreateConnection();
                con.Open();
            }

            _dbDataAdapter.SelectCommand.Connection = con;
            _dbDataAdapter.SelectCommand.Transaction = tran;

            recordsAffected = _dbDataAdapter.Fill(ds);

            if(dataSession == null) 
            {
                con.Close();
            }

            return recordsAffected;
        }

        public int Update(DataSet ds) 
        {
            if(_updateCommand == null &&
                _insertCommand == null &&
                _deleteCommand == null)
                throw new DataException("At least one of the commands Insert/Update/Delete should be defined before trying to Update a DataSet using IDataSetAdapter.");

            _dbDataAdapter.UpdateCommand = _updateCommand.DbCommand;
            _dbDataAdapter.InsertCommand = _insertCommand.DbCommand;
            _dbDataAdapter.DeleteCommand = _deleteCommand.DbCommand;

            DataSession dataSession = DataSessionFactory.GetDataSession(_dataSource);

            IDbConnection con = null;
            IDbTransaction tran = null;
            int recordsAffected = 0;

            if(dataSession != null) 
            {
                con = dataSession.Connection;
                tran = dataSession.Transaction;
            }
            else 
            {
                con = _dataSource.CreateConnection();
                con.Open();
            }

            if(_dbDataAdapter.UpdateCommand != null) 
            {
                _dbDataAdapter.UpdateCommand.Connection = con;
                _dbDataAdapter.UpdateCommand.Transaction = tran;
            }
            if(_dbDataAdapter.InsertCommand != null) 
            {
                _dbDataAdapter.InsertCommand.Connection = con;
                _dbDataAdapter.InsertCommand.Transaction = tran;
            }
            if(_dbDataAdapter.DeleteCommand != null) 
            {
                _dbDataAdapter.DeleteCommand.Connection = con;
                _dbDataAdapter.DeleteCommand.Transaction = tran;
            }
            
            recordsAffected = _dbDataAdapter.Update(ds);

            if(dataSession == null) 
            {
                con.Close();
            }

            return recordsAffected;
        }

        public IDataCommand SelectCommand 
        { 
            get { return _selectCommand; }
            set { _selectCommand = value; } 
        }
        public IDataCommand UpdateCommand
        { 
            get { return _updateCommand; }
            set { _updateCommand = value; } 
        }
        public IDataCommand InsertCommand
        { 
            get { return _insertCommand; }
            set { _insertCommand = value; } 
        }
        public IDataCommand DeleteCommand
        { 
            get { return _deleteCommand; }
            set { _deleteCommand = value; } 
        }

        public DataTableMappingCollection TableMappings 
        {
            get { return (DataTableMappingCollection)_dbDataAdapter.TableMappings; }
        }

        public void PopulateCommands() 
        {
            //TODO: check preconditions and raise exception if not fulfilled

            if(_commandBuilder == null) 
            {
                _commandBuilder = 
                    Activator.CreateInstance(_dataSource.Provider.CommandBuilderObjectType);
            }

            //here assign the selectcommand
            _dbDataAdapter.SelectCommand = _selectCommand.DbCommand;
            //and open a connection, because the CommandBuilders retrieves metadata from the db!
            IDbConnection con = _dataSource.CreateConnection();
            con.Open();
            _dbDataAdapter.SelectCommand.Connection = con;

            //TODO fix this exception handling hack
            try 
            {
                PropertyInfo selectCommandProperty = _commandBuilder.GetType().GetProperty("DataAdapter");
                selectCommandProperty.SetValue(_commandBuilder, _dbDataAdapter, null);

                //after that get the other commands, wrap them and assign them to the current datasetadapter
                MethodInfo getCommandMethod = _commandBuilder.GetType().GetMethod("GetInsertCommand");
                IDbCommand dbCommand = (IDbCommand)getCommandMethod.Invoke(_commandBuilder, null);
                _insertCommand = new DataCommand(dbCommand, _dataSource);
                getCommandMethod = _commandBuilder.GetType().GetMethod("GetDeleteCommand");
                dbCommand = (IDbCommand)getCommandMethod.Invoke(_commandBuilder, null);
                _deleteCommand = new DataCommand(dbCommand, _dataSource);
                getCommandMethod = _commandBuilder.GetType().GetMethod("GetUpdateCommand");
                dbCommand = (IDbCommand)getCommandMethod.Invoke(_commandBuilder, null);
                _updateCommand = new DataCommand(dbCommand, _dataSource);
            }
            catch(Exception e) 
            {
                Debug.WriteLine(e.ToString());
                throw;
            }
            finally 
            {
                con.Close();
                _dbDataAdapter.SelectCommand.Connection = null;
                _dbDataAdapter.SelectCommand = null;
            }

        }
    }
}
