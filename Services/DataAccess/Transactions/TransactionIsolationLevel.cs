using System;

namespace FindMyFamilies.Transactions
{
    /// <summary>
    /// Summary description for IsolationLevel.
    /// </summary>
    public enum TransactionIsolationLevel
    {
        ReadUncommitted,
        ReadCommitted,
        RepeatableRead,
        Serializable
    }
}
