using System;

namespace FindMyFamilies.Transactions {

    /// <summary>
    /// Summary description for TransactionContextState.
    /// </summary>
    public enum TransactionContextState {
        Created,
        Entered,
        Exitted,
        ToBeCommitted,
        ToBeRollbacked
    }
}
