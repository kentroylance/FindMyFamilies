using System;
using System.Data;
using System.Collections;
using System.Reflection;
using FindMyFamilies.Exceptions;

namespace  FindMyFamilies.DataAccess
{
    /// <summary>
    ///  tthis is a testDataParameterCollection contains DataCommand's IDbDataParameters
    /// </summary>
    public class DataParameterCollection : MarshalByRefObject, IDataParameterCollection, ICloneable
    {
        private IDbCommand _dbCommand = null;   //the command, the parameters of which are managed by this
        private IDataParameterCollection _parameters = null;    //the parameters
        private Hashtable _parameterKeyNames = new Hashtable();

        private string _parameterNamePrefix = "";
        private Type _parameterDbType = null;
        private PropertyInfo _parameterDbTypeProperty = null;

        internal IDictionary ParameterKeyNames 
        { 
            get { return _parameterKeyNames; } 
            set { _parameterKeyNames = (Hashtable)value; }
        }

        internal DataParameterCollection(
            IDbCommand dbCommand, Type parameterDbType, PropertyInfo parameterDbTypeProperty, 
            string parameterNamePrefix)
        {
            _dbCommand = dbCommand;
            _parameters = _dbCommand.Parameters;
            _parameterDbType = parameterDbType;
            _parameterDbTypeProperty = parameterDbTypeProperty;
            _parameterNamePrefix = parameterNamePrefix;
        }
        internal DataParameterCollection(
            IDbCommand dbCommand, Type parameterDbType, PropertyInfo parameterDbTypeProperty, 
            string parameterNamePrefix, Hashtable parameterKeyNames)
            : this(dbCommand, parameterDbType, parameterDbTypeProperty, parameterNamePrefix)
        {
            _parameterKeyNames = parameterKeyNames;
        }

        private string GetParamNameFromKey(string key) 
        {
            string result = "";

            if(_parameterKeyNames.Contains(key)) 
                result = (string)_parameterKeyNames[key];
            else if(key.IndexOf(_parameterNamePrefix) < 0)
                result = _parameterNamePrefix + key;
            else
                result = key;

            return result;
        }
        private string GetParamKeyFromName(string name) 
        {
            string result = "";

            if(_parameterNamePrefix == string.Empty) 
            {
                result = name;
            }
            else 
            {
                result = name.Replace(_parameterNamePrefix, "");
            }

            return result;
        }

        public IDbDataParameter Add(
            string paramKey, string paramName, Enum paramType, ParameterDirection paramDirection) 
        {
            IDbDataParameter param = _dbCommand.CreateParameter();

            if(paramKey == null || paramKey == string.Empty) 
                paramKey = GetParamKeyFromName(paramName);

            param.ParameterName = paramName;    

            if(paramType is DbType) 
            {
                param.DbType = (DbType)paramType;
            }
            else 
            {
                if(paramType.GetType() != _parameterDbType) 
                {
                    throw new DataAccessException("Invalid parameter type specified.");
                }

                _parameterDbTypeProperty.SetValue(param, paramType, null);
            }

            param.Direction = paramDirection;

            _parameters.Add(param);
            _parameterKeyNames.Add(paramKey, paramName);

            return param;
        }

        public IDbDataParameter Add(
            string paramName, Enum paramType, ParameterDirection paramDirection) 
        {
            string paramKey = GetParamKeyFromName(paramName);

            return Add(paramKey, paramName, paramType, paramDirection);
        }

        public IDbDataParameter Add(
            string paramName, Enum paramType, 
            ParameterDirection paramDirection, object paramValue)
        {
            string paramKey = GetParamKeyFromName(paramName);

            IDbDataParameter param = Add(paramKey, paramName, paramType, paramDirection, paramValue);

            return param;
        }

        public IDbDataParameter Add(
            string paramKey, string paramName, Enum paramType, 
            ParameterDirection paramDirection, object paramValue)
        {
            IDbDataParameter param = Add(paramKey, paramName, paramType, paramDirection);
            param.Value = paramValue;

            return param;
        }

        public IDbDataParameter Add(
            string paramName, Enum paramType, int paramSize,
            ParameterDirection paramDirection, object paramValue)
        {
            string paramKey = GetParamKeyFromName(paramName);

            IDbDataParameter param = Add(paramKey, paramName, paramType, paramSize, paramDirection, 
                paramValue);

            return param;
        }

        public IDbDataParameter Add(
            string paramKey, string paramName, Enum paramType, int paramSize,
            ParameterDirection paramDirection, object paramValue)
        {
            IDbDataParameter param = Add(paramKey, paramName, paramType, paramDirection, paramValue);
            param.Size = paramSize;

            return param;
        }

        public IDbDataParameter Add(
            string paramName, Enum paramType, int paramSize, ParameterDirection paramDirection) 
        {
            string paramKey = GetParamKeyFromName(paramName);

            IDbDataParameter param = Add(paramKey, paramName, paramType, paramSize, paramDirection);
            
            return param;
        }

        public IDbDataParameter Add(
            string paramKey, string paramName, Enum paramType, int paramSize, 
            ParameterDirection paramDirection) 
        {
            IDbDataParameter param = Add(paramKey, paramName, paramType, paramDirection);
            if(paramSize > 0)
                param.Size = paramSize;

            return param;
        }

        public IDbDataParameter Add(
            string paramName, Enum paramType, ParameterDirection paramDirection, 
            string sourceColumn, DataRowVersion sourceVersion) 
        {
            string paramKey = GetParamKeyFromName(paramName);

            IDbDataParameter param = Add(paramKey, paramName, paramType, paramDirection, 
                sourceColumn, sourceVersion);

            return param;
        }

        public IDbDataParameter Add(
            string paramKey, string paramName, Enum paramType, ParameterDirection paramDirection, 
            string sourceColumn, DataRowVersion sourceVersion) 
        {
            IDbDataParameter param = Add(paramKey, paramName, paramType, paramDirection, sourceColumn);
            param.SourceVersion = sourceVersion;

            return param;
        }


        public IDbDataParameter Add(
            string paramName, Enum paramType, ParameterDirection paramDirection, 
            string sourceColumn, DataRowVersion sourceVersion,
            object paramValue) 
        {
            string paramKey = GetParamKeyFromName(paramName);

            IDbDataParameter param = Add(paramName, paramType, paramDirection, 
                sourceColumn, sourceVersion, paramValue);

            return param;
        }

        public IDbDataParameter Add(
            string paramKey, string paramName, Enum paramType, ParameterDirection paramDirection, 
            string sourceColumn, DataRowVersion sourceVersion,
            object paramValue) 
        {
            IDbDataParameter param = Add(paramKey, paramName, paramType, paramDirection, sourceColumn, sourceVersion);
            param.Value = paramValue;

            return param;
        }

        public IDbDataParameter Add(
            string paramName, Enum paramType, int paramSize, ParameterDirection paramDirection, 
            string sourceColumn, DataRowVersion sourceVersion) 
        {
            string paramKey = GetParamKeyFromName(paramName);

            IDbDataParameter param = Add(paramName, paramType, paramSize, paramDirection, 
                sourceColumn, sourceVersion);

            return param;
        }

        public IDbDataParameter Add(
            string paramKey, string paramName, Enum paramType, int paramSize, 
            ParameterDirection paramDirection, string sourceColumn, DataRowVersion sourceVersion) 
        {
            IDbDataParameter param = Add(paramKey, paramName, paramType, paramSize, paramDirection, sourceColumn);
            param.SourceVersion = sourceVersion;

            return param;
        }

        public IDbDataParameter Add(
            string paramName, Enum paramType, int paramSize, ParameterDirection paramDirection, 
            string sourceColumn, DataRowVersion sourceVersion,
            object paramValue) 
        {
            string paramKey = GetParamKeyFromName(paramName);

            IDbDataParameter param = Add(paramKey, paramName, paramType, paramSize, paramDirection, 
                sourceColumn, sourceVersion, paramValue);

            return param;
        }

        public IDbDataParameter Add(
            string paramKey, string paramName, Enum paramType, int paramSize, ParameterDirection paramDirection, 
            string sourceColumn, DataRowVersion sourceVersion,
            object paramValue) 
        {
            IDbDataParameter param = Add(paramKey, paramName, paramType, paramSize, paramDirection, 
                sourceColumn, sourceVersion);
            param.Value = paramValue;

            return param;
        }

        public IDbDataParameter this[int parameterIndex]
        {
            get
            {
                return (IDbDataParameter)_parameters[parameterIndex];
            }
            set
            {
                _parameters[parameterIndex] = value;
            }
        }

        public IDbDataParameter this[string parameterKey]
        {
            get
            {
                return (IDbDataParameter)_parameters[GetParamNameFromKey(parameterKey)];
            }
            set
            {
                _parameters[GetParamNameFromKey(parameterKey)] = value;
            }
        }

        #region IDataParameterCollection Members

        object IDataParameterCollection.this[string parameterKey]
        {
            get
            {
                return _parameters[GetParamNameFromKey(parameterKey)];
            }
            set
            {
                _parameters[GetParamNameFromKey(parameterKey)] = value; 
            }
        }

        public void RemoveAt(string parameterKey)
        {
            _parameters.RemoveAt(GetParamNameFromKey(parameterKey));
        }

        public bool Contains(string parameterKey)
        {
            return _parameters.Contains(GetParamNameFromKey(parameterKey));
        }

        public int IndexOf(string parameterKey)
        {
            return _parameters.IndexOf(GetParamNameFromKey(parameterKey));
        }

        #endregion

        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _parameters.IsReadOnly;
            }
        }

        object System.Collections.IList.this[int index]
        {
            get
            {
                return _parameters[index];
            }
            set
            {
                _parameters[index] = value; 
            }
        }

        void System.Collections.IList.RemoveAt(int index)
        {
            _parameters.RemoveAt(index);
        }

        public void Insert(int index, object value)
        {
            _parameters.Insert(index, value);
        }

        public void Remove(object value)
        {
            _parameters.Remove(value);
        }

        bool System.Collections.IList.Contains(object value)
        {
            return _parameters.Contains(value);
        }

        public void Clear()
        {
            _parameters.Clear();
        }

        int System.Collections.IList.IndexOf(object value)
        {
            return _parameters.IndexOf(value);
        }

        int System.Collections.IList.Add(object value)
        {
            return _parameters.Add(value);
        }

        public bool IsFixedSize
        {
            get
            {
                return _parameters.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                return _parameters.IsSynchronized;
            }
        }

        public int Count
        {
            get
            {
                return _parameters.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _parameters.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _parameters.SyncRoot;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return _parameters.GetEnumerator();
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            return new DataParameterCollection(_dbCommand, _parameterDbType, 
                _parameterDbTypeProperty, _parameterNamePrefix, _parameterKeyNames);
        }

        #endregion
    }
}
