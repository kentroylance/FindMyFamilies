using System;
using System.Data;
using System.Collections;

namespace  FindMyFamilies.DataAccess {

	/// <summary>
	/// DataCommand wraps one (or more?) IDbCommands.
	/// </summary>
	class DataCommand : IDataCommand {

		private IDbCommand _dbCommand = null;
		private DataParameterCollection _parameters = null;
		private string _commandName = "";
		private DataSource _dataSource = null;

		public DataCommand(IDbCommand dbCommand, DataSource ds) {
			_dbCommand = dbCommand;
			_dataSource = ds;
			_parameters = new DataParameterCollection(dbCommand, ds.Provider.ParameterDbType, 
				ds.Provider.ParameterDbTypeProperty, ds.ParameterNamePrefix);
		}

		public DataCommand(string commandName, IDbCommand dbCommand, DataSource ds)
			: this(dbCommand, ds) {
			_commandName = commandName;
		}

		internal DataCommand(string commandName, IDbCommand dbCommand, DataSource ds, 
			IDictionary parameterKeyNames)
			: this (commandName, dbCommand, ds) {
			_parameters.ParameterKeyNames = parameterKeyNames;
		}

		public string Name { 
			get { return _commandName; }
			set { _commandName = value; } 
		}

		public IDataSource DataSource { get { return _dataSource; } }

		public DataParameterCollection Parameters { get { return _parameters; } }

//(IDbCommand)Activator.CreateInstance(_provider.CommandObjectType)
//		(IDbCommand)((ICloneable)_dbCommand).Clone()
		public object Clone() {
			//IDbCommand dataCommand = (IDbCommand)Activator.CreateInstance(_dataSource.Provider.CommandObjectType);
			IDbCommand dataCommand1 = (IDbCommand)((ICloneable)_dbCommand).Clone();

			return new DataCommand(
				_commandName, dataCommand1, _dataSource, 
				_parameters.ParameterKeyNames);
		}

		public int ExecuteNonQuery() {
			int recordsAffected = 0;

			DataSession dataSession = DataSessionFactory.GetDataSession(_dataSource);

			IDbConnection con = null;
			IDbTransaction tran = null;
            
			if(dataSession != null) {
				con = dataSession.Connection;
				tran = dataSession.Transaction;
			}

			else {
				con = _dataSource.CreateConnection();
				con.Open();
			}

			_dbCommand.Connection = con;
			_dbCommand.Transaction = tran;

			recordsAffected = _dbCommand.ExecuteNonQuery();

			if(dataSession == null) {
				con.Close();
			}

			return recordsAffected;
		}

		public int ExecuteNonQuery(IDbConnection con, IDbTransaction tran) {
			_dbCommand.Connection = con;
			_dbCommand.Transaction = tran;

			return _dbCommand.ExecuteNonQuery();
		}

		public IDataReader ExecuteReader() {
			//int recordsAffected = 0;

			DataSession dataSession = DataSessionFactory.GetDataSession(_dataSource);

			IDbConnection con = null;
			IDbTransaction tran = null;
			CommandBehavior commandBehaviour = CommandBehavior.Default;
            
			if(dataSession != null) {
				con = dataSession.Connection;
				tran = dataSession.Transaction;
			}

			else {
				con = _dataSource.CreateConnection();
				System.Data.ConnectionState test = con.State;
				string test1 = test.ToString();
//				if (con.State) {
//				}
				con.Open();
				commandBehaviour = CommandBehavior.CloseConnection;
			}

			_dbCommand.Connection = con;
			_dbCommand.Transaction = tran;

			IDataReader dr = _dbCommand.ExecuteReader(commandBehaviour);

			return dr;
		}

		public IDbCommand DbCommand {
			get { return _dbCommand; }
			//set { _dbCommand = value; }
		}

		public void SetParameter(string fieldName, object dataValue) {
			fieldName = "@" + fieldName;
			DbType dbType = this.Parameters[fieldName].DbType;
			if (dbType.Equals(DbType.AnsiString) || dbType.Equals(DbType.String)) {
				if (dataValue == null) {
					dataValue = "";
				}
				this.Parameters[fieldName].Value = (string)dataValue;
			} else if (dbType.Equals(DbType.Int32) || dbType.Equals(DbType.Int16)) {
				if (dataValue == null) {
					dataValue = 0;
				}
				this.Parameters[fieldName].Value = (int)dataValue;
			} else if (dbType.Equals(DbType.Double)) {
				if (dataValue == null) {
					dataValue = 0.0;
				}
				this.Parameters[fieldName].Value = (double)dataValue;
			} else if (dbType.Equals(DbType.Boolean)) {
				int newValue = 0;
				if (dataValue != null) {
					if ((bool)dataValue) {
						newValue = 1;
					}
				}
				this.Parameters[fieldName].Value = newValue;
			} else if (dbType.Equals(DbType.DateTime)) {
				if ((dataValue == null) || ((dataValue != null) && ((DateTime)dataValue).ToShortDateString().Equals("1/1/0001"))) {
					dataValue = new DateTime(1900, 1, 1);
				}
				this.Parameters[fieldName].Value = (DateTime)dataValue;
			} else {
				this.Parameters[fieldName].Value = dataValue;
			}
		}
	}
}
