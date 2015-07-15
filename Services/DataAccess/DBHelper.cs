using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Xml;
using FindMyFamilies.Util;

namespace  FindMyFamilies.DataAccess {
    public class DBHelper {
        public const string DB_COMBO_TEXT = "DbComboText";
        public const string DB_COMBO_VALUE = "DbComboValue";
        private static string connectionString;
        private readonly Logger logger = new Logger(MethodBase.GetCurrentMethod().DeclaringType);

        private SqlConnection connection;


        public DBHelper() {
            // Initialize the class' members.
            InitClass();
        }

        private void InitClass() {
            // create all the objects and initialize other members.
        }

        protected bool getBoolean(SqlDataReader reader, string fieldName) {
            var returnValue = (bool) reader[fieldName];
            return returnValue;
        }

        protected int getInt(SqlDataReader reader, string fieldName) {
            Object value = reader[fieldName];
            int returnvalue = 0;
            if (value is Byte) {
                returnvalue = Convert.ToInt16(value);
            } else if (value is Decimal) {
                returnvalue = Convert.ToInt32(value);
            } else if (value is Int16) {
                var dataValue = (Int16) value;
                returnvalue = dataValue;
            } else if (value is Int32) {
                var dataValue = (Int32) value;
                returnvalue = dataValue;
            }
            //decimal dataValue = (decimal)reader[fieldName];
            //return (int)dataValue;
            return returnvalue;
        }

        protected int getInt(SqlDataReader reader, int column) {
            var dataValue = (decimal) reader[column];
            return (int) dataValue;
        }

        protected int getInt16(SqlDataReader reader, string fieldName) {
            var dataValue = (Int16) reader[fieldName];
            return dataValue;
        }

        protected int getInt16(SqlDataReader reader, int column) {
            var dataValue = (Int16) reader[column];
            return dataValue;
        }

        protected int getInt32(SqlDataReader reader, string fieldName) {
            var dataValue = (Int32) reader[fieldName];
            return dataValue;
        }

        protected int getInt32(SqlDataReader reader, int column) {
            var dataValue = (Int32) reader[column];
            return dataValue;
        }

        protected string getString(SqlDataReader reader, string fieldName) {
            return (string) reader[fieldName];
        }

        protected string getString(SqlDataReader reader, int column) {
            return (string) reader[column];
        }

        protected DateTime getDate(SqlDataReader reader, string fieldName) {
            var dateValue = (DateTime) reader[fieldName];
            if (dateValue.ToShortDateString().Equals("1/1/1900")) {
                dateValue = new DateTime(0001, 1, 1);
            }
            return dateValue;
        }


        protected int setInt(bool value) {
            int returnValue = 0;
            if (value) {
                returnValue = 1;
            }
            return returnValue;
        }

        protected string setString(string value) {
            if (value == null) {
                value = "";
            }
            return value;
        }

        protected DateTime setDate(DateTime value) {
            string stringValue = value.ToShortDateString();
            if (value.ToShortDateString().Equals("1/1/0001")) {
                value = new DateTime(1900, 1, 1);
            }
            return value;
        }


        /// <summary>
        ///     Query the database with no parameters
        /// </summary>
        /// <param name="procName">Name of stored procedure.</param>
        /// <returns>Data reader.</returns>
        public SqlDataReader getList(string procName) {
            SqlDataReader dataReader = null;

            try {
                runProc(procName, out dataReader);
            } catch (Exception ex) {
                logger.Error(ex);
            }

            return dataReader;
        }

        public ICollection getDropDownList(string procName) {
            return getDropDownList(procName, null);
        }

        public ICollection getDropDownList(string procName, SqlParameter[] sqlParams) {
            SqlDataReader dataReader = null;
            IList itemCollection = new ListItemCollection();
            ListItem listItem;

            try {
                if (sqlParams != null) {
                    runProc(procName, sqlParams, out dataReader);
                } else {
                    runProc(procName, out dataReader);
                }
                while (dataReader.Read()) {
                    string value = Convert.ToString(getInt(dataReader, 0));
                    listItem = new ListItem(getString(dataReader, 1), Convert.ToString(getInt(dataReader, 0)));
                    itemCollection.Add(listItem);
                }
                dataReader.Close();
            } catch (Exception ex) {
                logger.Error(ex);
            } finally {
                dataReader.Close();
            }
            return itemCollection;
        }

        /// <summary>
        ///     Query the database passing in an ID parameter
        /// </summary>
        /// <param name="idValue">Value of the primary key in the table.</param>
        /// <param name="procName">Name of stored procedure.</param>
        /// <returns>Data reader.</returns>
        public SqlDataReader getListById(string procName, int idValue) {
            SqlDataReader dataReader = null;

            try {
                SqlParameter[] sqlParams = {
                    makeInParam("@id", SqlDbType.Image, 10, idValue)
                };
                runProc(procName, sqlParams, out dataReader);
            } catch (Exception ex) {
                logger.Error(ex);
            }

            return dataReader;
        }

        /// <summary>
        ///     Query the database passing in an ID parameter
        /// </summary>
        /// <param name="idValue">Value of the primary key in the table.</param>
        /// <param name="procName">Name of stored procedure.</param>
        /// <returns>Data reader.</returns>
        public SqlDataReader getListById(string procName, string idFieldName, int idValue) {
            SqlDataReader dataReader = null;

            try {
                SqlParameter[] sqlParams = {
                    makeInParam("@" + idFieldName.Trim(), SqlDbType.Image, 10, idValue)
                };
                runProc(procName, sqlParams, out dataReader);
            } catch (Exception ex) {
                logger.Error(ex);
            }

            return dataReader;
        }

        /// <summary>
        ///     Query the database by passing a list of input parameters.
        /// </summary>
        /// <param name="idValue">Value of the primary key in the table.</param>
        /// <param name="procName">Name of stored procedure.</param>
        /// <returns>Data reader.</returns>
        public SqlDataReader getList(string procName, SqlParameter[] sqlParams) {
            SqlDataReader dataReader = null;

            try {
                if (sqlParams == null) {
                    runProc(procName, out dataReader);
                } else {
                    runProc(procName, sqlParams, out dataReader);
                }
            } catch (Exception ex) {
                logger.Error(ex);
            }

            return dataReader;
        }

        /// <summary>
        ///     Run stored procedure.
        /// </summary>
        /// <param name="procName">Name of stored procedure.</param>
        /// <returns>Stored procedure return value.</returns>
        protected int runProc(string procName) {
            SqlCommand cmd = createCommand(procName, null);
            cmd.ExecuteNonQuery();
            close();
            return (int) cmd.Parameters["ReturnValue"].Value;
        }

        /// <summary>
        ///     Run stored procedure.
        /// </summary>
        /// <param name="procName">Name of stored procedure.</param>
        /// <returns>Stored procedure return value.</returns>
        protected void runProcNoReturn(string procName) {
            SqlCommand cmd = createCommand(procName, null);
            cmd.ExecuteNonQuery();
            close();
        }

        /// <summary>
        ///     Run stored procedure.
        /// </summary>
        /// <param name="procName">Name of stored procedure.</param>
        /// <param name="prams">Stored procedure params.</param>
        /// <returns>Stored procedure return value.</returns>
        protected int runProc(string procName, SqlParameter[] prams) {
            SqlCommand cmd = createCommand(procName, prams);
            cmd.ExecuteNonQuery();
            close();
            return (int) cmd.Parameters["ReturnValue"].Value;
        }

        /// <summary>
        ///     Run stored procedure.
        /// </summary>
        /// <param name="procName">Name of stored procedure.</param>
        /// <param name="prams">Stored procedure params.</param>
        /// <returns>Stored procedure return value.</returns>
        protected void runProcNoReturn(string procName, SqlParameter[] prams) {
            SqlCommand cmd = createCommand(procName, prams);
            cmd.ExecuteNonQuery();
            close();
        }

        /// <summary>
        ///     Run stored procedure.
        /// </summary>
        /// <param name="procName">Name of stored procedure.</param>
        /// <param name="dataReader">Return result of procedure.</param>
        protected void runProc(string procName, out SqlDataReader dataReader) {
            SqlCommand cmd = createCommand(procName, null);
            dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        ///     Run stored procedure.
        /// </summary>
        /// <param name="procName">Name of stored procedure.</param>
        /// <param name="dataReader">Return result of procedure.</param>
        protected string runProcReturnString(string procName, SqlParameter[] input, SqlParameter[] output) {
            SqlCommand cmd = createCommand(procName, input, output);
            cmd.ExecuteNonQuery();
            var returnValue = (string) cmd.Parameters[output[0].ParameterName].Value;
            close();
            return returnValue;
        }

        /// <summary>
        ///     Run stored procedure.
        /// </summary>
        /// <param name="procName">Name of stored procedure.</param>
        /// <param name="dataReader">Return result of procedure.</param>
        protected object runProc(string procName, SqlParameter[] prams, string fieldName) {
            SqlCommand cmd = createCommandPrams(procName, prams);
            cmd.ExecuteNonQuery();
            if (fieldName.IndexOf("@", 0, 1) < 0) {
                fieldName = "@" + fieldName;
            }
            object returnValue = cmd.Parameters[fieldName].Value;
            close();
            return returnValue;
        }

        /// <summary>
        ///     Run stored procedure.
        /// </summary>
        /// <param name="procName">Name of stored procedure.</param>
        /// <param name="prams">Stored procedure params.</param>
        /// <param name="dataReader">Return result of procedure.</param>
        protected void runProc(string procName, SqlParameter[] prams, out SqlDataReader dataReader) {
            SqlCommand cmd = createCommand(procName, prams);
            dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        ///     Create command object used to call stored procedure.
        /// </summary>
        /// <param name="procName">Name of stored procedure.</param>
        /// <param name="prams">Params to stored procedure.</param>
        /// <returns>Command object.</returns>
        protected SqlCommand createCommand(string procName, SqlParameter[] prams) {
            // make sure connection is open
            open();

            //command = new SqlCommand( sprocName, new SqlConnection( ConfigManager.DALConnectionString ) );
            var cmd = new SqlCommand(procName, connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // add proc parameters
            if (prams != null) {
                foreach (SqlParameter parameter in prams) {
                    cmd.Parameters.Add(parameter);
                }
            }

            // return param
            cmd.Parameters.Add(new SqlParameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.ReturnValue, false, 0, 0, string.Empty,
                DataRowVersion.Default, null));

            return cmd;
        }

        /// <summary>
        ///     Create command object used to call stored procedure.
        /// </summary>
        /// <param name="procName">Name of stored procedure.</param>
        /// <param name="prams">Params to stored procedure.</param>
        /// <returns>Command object.</returns>
        private SqlCommand createCommand(string procName, SqlParameter[] input, SqlParameter[] output) {
            // make sure connection is open
            open();

            //command = new SqlCommand( sprocName, new SqlConnection( ConfigManager.DALConnectionString ) );
            var cmd = new SqlCommand(procName, connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // add input proc parameters
            if (input != null) {
                foreach (SqlParameter parameter in input) {
                    cmd.Parameters.Add(parameter);
                }
            }

            // add output proc parameter
            if (output != null) {
                foreach (SqlParameter parameter in output) {
                    cmd.Parameters.Add(parameter);
                }
            }

            return cmd;
        }

        /// <summary>
        ///     Create command object used to call stored procedure.
        /// </summary>
        /// <param name="procName">Name of stored procedure.</param>
        /// <param name="prams">Params to stored procedure.</param>
        /// <returns>Command object.</returns>
        private SqlCommand createCommandPrams(string procName, SqlParameter[] prams) {
            // make sure connection is open
            open();

            //command = new SqlCommand( sprocName, new SqlConnection( ConfigManager.DALConnectionString ) );
            var cmd = new SqlCommand(procName, connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // add parameters
            if (prams != null) {
                foreach (SqlParameter parameter in prams) {
                    cmd.Parameters.Add(parameter);
                }
            }

            return cmd;
        }

        /// <summary>
        ///     Open the connection.
        /// </summary>
        protected void open() {
            // open connection
            if (connection == null) {
                if (connectionString == null) {
                    connectionString = Constants.CONNECTION_STRING;
                }
                if (connectionString == null) {
                    loadConfig();
                }
                connection = new SqlConnection(connectionString);
            }
            if (connection.State.ToString().Equals("Closed")) {
                connection.Open();
            }
        }

        private void loadConfig() {
            XmlTextReader reader = null;
            try {
                reader = new XmlTextReader("DBConfig.xml");
                reader.WhitespaceHandling = WhitespaceHandling.None;

                while (reader.Read()) {
                    if (reader.NodeType == XmlNodeType.Element) {
                        if (reader.Name.Equals("connection-string")) {
                            connectionString = reader.ReadString();
                        }
                    }
                }
                reader.Close();
            } catch (Exception e) {
                throw new IOException("Cannot load page config properties from xml. " + e.Message);
            } finally {
                reader.Close();
            }
        }


        /// <summary>
        ///     Close the connection.
        /// </summary>
        protected void close() {
            if (connection != null) {
                connection.Close();
            }
        }

        /// <summary>
        ///     Release resources.
        /// </summary>
        protected void dispose() {
            // make sure connection is closed
            if (connection != null) {
                connection.Dispose();
                connection = null;
            }
        }

        /// <summary>
        ///     Make input param.
        /// </summary>
        /// <param name="ParamName">Name of param.</param>
        /// <param name="DbType">Param type.</param>
        /// <param name="Size">Param size.</param>
        /// <param name="Value">Param value.</param>
        /// <returns>New parameter.</returns>
        protected SqlParameter makeInParam(string ParamName, SqlDbType DbType, int Size, object Value) {
            return makeParam(ParamName, DbType, Size, ParameterDirection.Input, Value);
        }

        /// <summary>
        ///     Make input param.
        /// </summary>
        /// <param name="ParamName">Name of param.</param>
        /// <param name="DbType">Param type.</param>
        /// <param name="Size">Param size.</param>
        /// <returns>New parameter.</returns>
        protected SqlParameter makeOutParam(string ParamName, SqlDbType DbType, int Size) {
            return makeParam(ParamName, DbType, Size, ParameterDirection.Output, 000000);
        }

        /// <summary>
        ///     Make stored procedure param.
        /// </summary>
        /// <param name="ParamName">Name of param.</param>
        /// <param name="DbType">Param type.</param>
        /// <param name="Size">Param size.</param>
        /// <param name="Direction">Parm direction.</param>
        /// <param name="Value">Param value.</param>
        /// <returns>New parameter.</returns>
        protected SqlParameter makeParam(string ParamName, SqlDbType DbType, Int32 Size, ParameterDirection Direction, object Value) {
            SqlParameter param;

            if (Size > 0) {
                param = new SqlParameter(ParamName, DbType, Size);
            } else {
                param = new SqlParameter(ParamName, DbType);
            }

            param.Direction = Direction;
            if (!(Direction == ParameterDirection.Output && Value == null)) {
                param.Value = Value;
            }

            return param;
        }

        protected static SqlParameter[] makeInputParam(string paramName, SqlDbType dbType, Int32 size, object dataValue) {
            var param = new SqlParameter(paramName, dbType, size);
            param.Value = dataValue;
            SqlParameter[] prams = {
                param
            };
            return prams;
        }

        protected void delete(string procName, int idValue) {
            SqlParameter[] prams = {
                makeInParam("@id", SqlDbType.Int, 10, idValue)
            };

            try {
                runProc(procName, prams);
            } catch (Exception ex) {
                logger.Error(ex);
            }
        }

        protected void delete(string procName, SqlParameter[] prams) {
            try {
                runProc(procName, prams);
            } catch (Exception ex) {
                logger.Error(ex);
            }
        }

        protected SqlDataReader readAll(string procName) {
            // create data object and params
            SqlDataReader dataReader = null;
            try {
                // run the stored procedure and return an ADO.NET DataReader
                runProc(procName, out dataReader);
            } catch (Exception ex) {
                logger.Error(ex);
            }
            return dataReader;
        }

        /// <summary>
        ///     Updates a row in a table
        /// </summary>
        /// <param name="procName">string procName.</param>
        /// <param name="sqlParams">SqlParameter[] sqlParams.</param>
        protected void update(string procName, SqlParameter[] sqlParams) {
            // run the stored proc
            try {
                runProc(procName, sqlParams);
            } catch (Exception ex) {
                logger.Error(ex);
            }
        }

        /// <summary>
        ///     Creates a new row in a table.
        ///     returned, otherwise null is returned to the caller.</returns>
        ///     <param name="procName">string procName.</param>
        ///     <param name="sqlParams">SqlParameter[] sqlParams.</param>
        protected int create(string procName, SqlParameter[] sqlParams) {
            int idValue = 0;
            try {
                // run the stored procedure and return an ADO.NET DataReader
                idValue = runProc(procName, sqlParams);
            } catch (Exception ex) {
                logger.Error(ex);
            }
            return idValue;
        }

        /// <summary>
        ///     Creates a new row in a table.
        ///     returned, otherwise null is returned to the caller.</returns>
        ///     <param name="procName">string procName.</param>
        ///     <param name="sqlParams">SqlParameter[] sqlParams.</param>
        protected int create(string procName, SqlParameter[] sqlParams, string fieldName) {
            int idValue = 0;
            try {
                // run the stored procedure and return an ADO.NET DataReader
                idValue = (int) runProc(procName, sqlParams, fieldName);
            } catch (Exception ex) {
                logger.Error(ex);
            }
            return idValue;
        }

        // used for paging
        protected void positionReader(SqlDataReader reader, int currentPage, ref int recordCount, int pageSize, ref int recordPosition) {
            reader.Read();
            recordCount = reader.GetInt32(0);
            reader.NextResult();
            int start = 0;
            // now loop through the list and pull out items of the specified page
            if (!((pageSize >= recordCount) || (recordPosition + pageSize >= recordCount))) {
                start = (currentPage - 1)*pageSize;
                if (start <= 0) {
                    start = 1;
                }
                if (start < recordPosition) {
                    recordPosition = start;
                }
            } else {
                start = 1;
                recordPosition = start;
            }

            // skip 
            for (int i = 0; i < start - 1; i++) {
                if (!reader.Read()) {
                    break;
                }
                recordPosition = recordPosition + 1;
            }
            if (start > 1) {
                if (reader.Read()) {
                    recordPosition = recordPosition + 1;
                }
            }
        }
    }
}