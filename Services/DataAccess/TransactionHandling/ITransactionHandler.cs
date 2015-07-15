using System;
using FindMyFamilies.DataAccess;
using FindMyFamilies.Transactions;

namespace  FindMyFamilies.TransactionHandling
{
    /// <summary>
    /// Summary description for ITransactionHandler.
    /// </summary>
    public interface ITransactionHandler
    {
        void HandleTCCreated(object sender, TCCreatedEventArgs args);
        void HandleTCStateChangedEvent(object sender, TCStateChangedEventArgs args);

        DataSession GetDataSession(IDataSource ds);
    }
}
