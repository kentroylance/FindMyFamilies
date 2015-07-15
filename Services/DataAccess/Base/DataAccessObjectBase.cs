using System;
using System.Collections;
using System.Data;
using FindMyFamilies.Data;
using FindMyFamilies.Exceptions;
using FindMyFamilies.Util;

namespace  FindMyFamilies.DataAccess {
    /// <summary>
    ///     Summary description for DataAccessObject base class.
    /// </summary>
    public class DataAccessObjectBase {
        //end code addition

        public const string LANGUAGE = "Language_ID";
        protected IDataSource accountingDataSource = null;
        protected IDataSource dataSource = null;

        public DataAccessObjectBase() {
            dataSource = DataSourceFactory.GetDataSource(SystemInfo.Instance.ApplicationName);
        }

        public DataAccessObjectBase(string dataSourceName) {
            dataSource = DataSourceFactory.GetDataSource(dataSourceName);
        }

        public DataAccessObjectBase(IDataSource p_DataSource) {
            dataSource = p_DataSource;
        }

        public static string GetLanguage(IList dataTransferObjects) {
            string language = null;
            if ((dataTransferObjects != null) && (dataTransferObjects.Count > 0)) {
                if (dataTransferObjects[0] is DataTransferObject) {
                    language = ((DataTransferObject) dataTransferObjects[0]).Language;
                }
            }
            return language;
        }

        public void ProcessResult(IDataReader reader, DataTransferObject dataTransferObject, object logCategory) {
            ProcessResult(reader, null, dataTransferObject, null, logCategory);
        }

        public void ProcessResult(IDataReader reader, object primaryKey, DataTransferObject dataTransferObject, string operation, object logCategory) {
            if (!Strings.IsEmpty(operation) && operation.Equals(Constants.OPERATION_CREATE)) {
                if ((primaryKey != null) && (primaryKey is int) && ((int) primaryKey < 1)) {
                    dataTransferObject.OperationStatus = DataTransferObject.FAILED;
                    string errorMessage = Resource.GetErrorMessage(MessageKeys.INFO_INVALID_PRIMARY_KEY, dataTransferObject.Language);
                    throw new DataAccessException(errorMessage, null, logCategory);
                }
                dataTransferObject.OperationStatus = DataTransferObject.SUCCESSFUL;
            }
            if (reader.NextResult()) {
                reader.Read();
                int sqlError = reader.GetInt32(0);
                if (sqlError > 0) {
                    string errorMessage = Resource.GetErrorMessage(MessageKeys.INFO_INVALID, dataTransferObject.Language);
                    throw new DataAccessException(errorMessage, null, logCategory);
                }
                dataTransferObject.OperationStatus = DataTransferObject.SUCCESSFUL;
            }
        }

        public object GetValue(IDataReader reader, int column) {
            object dataValue = reader[column];
            Type dataType = reader.GetFieldType(column);
            if (dataType.Equals(typeof (String))) {
                if (dataValue.Equals(null) || (dataValue == DBNull.Value)) {
                    dataValue = "";
                }
            } else if (dataType.Equals(typeof (Int32))) {
                if (dataValue.Equals(null) || (dataValue == DBNull.Value)) {
                    dataValue = 0;
                } else {
                    dataValue = (int) dataValue;
                }
            } else if (dataType.Equals(typeof (Boolean))) {
                if (dataValue.Equals(null) || (dataValue == DBNull.Value)) {
                    dataValue = false;
                }
            } else if (dataType.Equals(typeof (DateTime))) {
                if (dataValue.Equals(null) || (dataValue == DBNull.Value)) {
                    dataValue = new DateTime(1900, 1, 1);
                } else if (((DateTime) dataValue).ToShortDateString().Equals("1/1/0001")) {
                    dataValue = new DateTime(1900, 1, 1);
                }
            } else if (dataType.Equals(typeof (Double))) {
                if (dataValue.Equals(null) || (dataValue == DBNull.Value)) {
                    dataValue = 0.0;
                } else {
                    dataValue = (double) dataValue;
                }
            } else if (dataType.Equals(typeof (Byte[]))) {
            } else {
                throw new Exception("Attempting to get an invalid data type " + dataType);
            }
            return dataValue;
        }

        public object GetValue(IDataReader reader, string fieldName) {
            object dataValue = reader[fieldName];
            int column = reader.GetOrdinal(fieldName);
            Type dataType = reader.GetFieldType(column);
            if (dataType.Equals(typeof (String))) {
                if (dataValue.Equals(null) || (dataValue == DBNull.Value)) {
                    dataValue = "";
                }
            } else if (dataType.Equals(typeof (Int32))) {
                if (dataValue.Equals(null) || (dataValue == DBNull.Value)) {
                    dataValue = 0;
                } else {
                    dataValue = (int) dataValue;
                }
            } else if (dataType.Equals(typeof (Boolean))) {
                if (dataValue.Equals(null) || (dataValue == DBNull.Value)) {
                    dataValue = false;
                }
            } else if (dataType.Equals(typeof (DateTime))) {
                if (dataValue.Equals(null) || (dataValue == DBNull.Value)) {
                    dataValue = new DateTime(1900, 1, 1);
                } else if (((DateTime) dataValue).ToShortDateString().Equals("1/1/0001")) {
                    dataValue = new DateTime(1900, 1, 1);
                }
            } else if (dataType.Equals(typeof (Double))) {
                if (dataValue.Equals(null) || (dataValue == DBNull.Value)) {
                    dataValue = 0.0;
                } else {
                    dataValue = (double) dataValue;
                }
            } else if (dataType.Equals(typeof (Byte[]))) {
            } else {
                throw new Exception("Attempting to get an invalid data type " + dataType);
            }
            return dataValue;
        }
    }
}