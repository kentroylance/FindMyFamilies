using System;
using System.Data;

namespace  FindMyFamilies.DataAccess
{
    /// <summary>
    /// DataSession simply holds a connection with its transaction.
    /// </summary>
    public class DataSession
    {
        private IDbConnection _con = null;
        private IDbTransaction _tran = null;

        public DataSession(IDbConnection con, IDbTransaction tran) 
        {
            _con = con;
            _tran = tran;
        }

        public IDbConnection Connection { get { return _con; } }
        public IDbTransaction Transaction { get { return _tran; } }
    }
}
