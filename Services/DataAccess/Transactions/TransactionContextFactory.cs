using System;
using FindMyFamilies.Exceptions;

namespace FindMyFamilies.Transactions {

    // A delegate type for hooking up transaction context state notifications.
    public delegate void TCCreatedEventHandler(object sender, TCCreatedEventArgs e);

    /// <summary>
    /// A customized EventArgs holding the created transaction context
    /// </summary>
    public class TCCreatedEventArgs : EventArgs {

        public TransactionContext Context;

        public TCCreatedEventArgs(TransactionContext context) {
            this.Context = context;
        }
    }

    /// <summary>
    /// TransactionContextFactory creates different transaction contexts 
    /// and raises a ContextCreated event.
    /// </summary>
    public class TransactionContextFactory {

        private TransactionContextFactory() {}

        // An event that clients can use to be notified whenever the
        // the a new transaction context is created.
        public static event TCCreatedEventHandler ContextCreated;

        public static TransactionContext GetContext(TransactionAffinity transactionAffinity) {
            TransactionContext ctx = null;

            switch(transactionAffinity) {

                case TransactionAffinity.RequiresNew:
                    ctx = new RequiresNewTransactionContext();
                    break;

                case TransactionAffinity.Required:
                    ctx = new RequiredTransactionContext();
                    break;

                case TransactionAffinity.Supported:
                    ctx = new SupportedTransactionContext();
                    break;

                case TransactionAffinity.NotSupported:
                    ctx = new NotSupportedTransactionContext();
                    break;
                default:
                    throw new TransactionContextException(transactionAffinity.ToString() + "is not currently supported.");
            }

            if(ContextCreated != null)
                ContextCreated(null, new TCCreatedEventArgs(ctx));

            return ctx;
        }

        public static TransactionContext GetCurrentContext() {
            return TransactionContext.Current;
        }
    }
}
