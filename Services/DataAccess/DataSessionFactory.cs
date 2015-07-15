using  FindMyFamilies.TransactionHandling;

namespace  FindMyFamilies.DataAccess {
    /// <summary>
    ///     DataSessionFactory will manage the interaction with the Transactions
    ///     looked from the Data Access perspective.
    /// </summary>
    internal class DataSessionFactory {
        private static readonly ITransactionHandler _transactionHandler;

        static DataSessionFactory() {
            //here choose the strategy based on the config file
            //_transactionHandler = new HomeGrownTransactionHandler();
            _transactionHandler = TransactionHandlerFactory.GetHandler();
        }

        private DataSessionFactory() {
        }

        internal static DataSession GetDataSession(IDataSource ds) {
            return _transactionHandler.GetDataSession(ds);
        }
    }
}