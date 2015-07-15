namespace FindMyFamilies.Util
{
    public interface ILogger
    {
        /// <summary>
        /// Writes a log entry when entering the method.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        void Enter(string methodName);

        /// <summary>
        /// Writes a log entry when leaving the method.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        void Leave(string methodName);

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="message">The message.</param>
        void Error(string message);

        /// <summary>
        /// Logs the warning message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Warning(string message);

        /// <summary>
        /// Logs the info message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Info(string message);
    }
}
