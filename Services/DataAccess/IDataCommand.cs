using System;
using System.Data;
using System.Collections;

namespace  FindMyFamilies.DataAccess
{
    /// <summary>
    /// Summary description for IDataCommand.
    /// </summary>
    public interface IDataCommand : ICloneable
    {
        string Name { get; set; }
        IDataSource DataSource { get; }

        IDbCommand DbCommand { get; }

        void SetParameter(string fieldName, object value);
        int ExecuteNonQuery();
        int ExecuteNonQuery(IDbConnection con, IDbTransaction tran);
        IDataReader ExecuteReader(); 

        DataParameterCollection Parameters { get; }
    }
}



