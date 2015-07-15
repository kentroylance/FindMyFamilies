using System;

using FindMyFamilies.Data;
using FindMyFamilies.Util;

namespace FindMyFamilies.Exceptions {

    /// <summary>
    /// Summary description for TransactionContextException.
    /// </summary>
    public class TransactionContextException : BaseException {

        public TransactionContextException() {}

        public TransactionContextException(string message) : base(message) {}

        public TransactionContextException(string message, Exception e) : base(message, e) {}

        public TransactionContextException(string message, Exception e, object logCategory) : base(message, e, logCategory) {}

        public TransactionContextException(string message, Exception e, object logCategory, DataTransferObject dataTransferObject) : base(message, e, logCategory, dataTransferObject) {}
    }
}
