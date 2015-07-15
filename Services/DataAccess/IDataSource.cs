using System;
using System.Data;

namespace  FindMyFamilies.DataAccess
{
    /// <summary>
    /// Summary description for IDataSource.
    /// </summary>
    public interface IDataSource
    {
        string Name { get; }

        IDbConnection CreateConnection();

        IDataCommand CreateCommand();
        IDataCommand CreateCommand(string commandText, CommandType commandType);
        IDataCommand GetCommand(string commandName);

        IDataSetAdapter CreateDataSetAdapter();
        IDataSetAdapter CreateDataSetAdapter(IDataCommand selectCommand);
        IDataSetAdapter GetDataSetAdapter(string adapterName);
    }
}
