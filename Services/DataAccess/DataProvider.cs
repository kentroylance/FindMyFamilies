using System;
using System.Reflection;

namespace  FindMyFamilies.DataAccess
{
    /// <summary>
    /// DataProvider contains the specific provider connection/command/param etc types
    /// to be instantiated later using late binding.
    /// </summary>
    class DataProvider
    {
        private string _name = "";
        private Type _connectionObjectType = null;
        private Type _commandObjectType = null;
        private Type _paramDbType = null;
        private PropertyInfo _paramDbTypeProperty = null;
        private Type _dataAdapterObjectType = null;
        private Type _commandBuilderObjectType = null;
        private string _parameterNamePrefix = "";

        public DataProvider(
            string name, Type connectionType, Type commandType, 
            Type paramType, Type paramDbType, PropertyInfo paramDbTypeProperty,
            Type dataAdapterType, Type commandBuilderObjectType, string parameterNamePrefix)
        {
            _name = name;
            _connectionObjectType = connectionType;
            _commandObjectType = commandType;
            
            _paramDbType = paramDbType;
            _paramDbTypeProperty = paramDbTypeProperty;

            _dataAdapterObjectType = dataAdapterType;
            _commandBuilderObjectType = commandBuilderObjectType;
            _parameterNamePrefix = parameterNamePrefix;
        }

        public Type ConnectionObjectType { get { return _connectionObjectType; } }
        public Type CommandObjectType { get { return _commandObjectType; } }
        public Type ParameterDbType { get { return _paramDbType; } }
        public PropertyInfo ParameterDbTypeProperty { get { return _paramDbTypeProperty; } }
        public Type DataAdapterObjectType{ get { return _dataAdapterObjectType; } }
        public Type CommandBuilderObjectType{ get { return _commandBuilderObjectType; } }
        public string ParameterNamePrefix{ get { return _parameterNamePrefix; } }

    }

}
