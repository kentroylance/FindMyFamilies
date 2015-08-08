using System;
using System.Runtime.Serialization;
using FindMyFamilies.Data;

namespace FindMyFamilies.Exceptions {
	/// <summary>
	/// Summary description for DataAccessException.
	/// </summary>
	public class DataAccessException : BaseException {

		public DataAccessException() {
		}

		public DataAccessException(string message) : base(message) {
		}

		public DataAccessException(string message, Exception e) : base(message, e) {
		}

		public DataAccessException(string message, Exception e, object logCategory) : base(message, e, logCategory) {
		}

		public DataAccessException(string message, Exception e, object logCategory, DataTransferObject dataTransferObject) : base(message, e, logCategory, dataTransferObject) {
		}

		public DataAccessException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}

		/// <summary>
		/// Allow inner exception to be created to create exception chaining
		/// </summary>
		/// <param name="message"></param>
		/// <param name="innerException"></param>
		public DataAccessException(string message, Exception exception, object logCategory, SessionDO session) : base(message, exception) {
		    this.Session = session;
		}


		public override void GetObjectData(SerializationInfo info, StreamingContext context) {
			base.GetObjectData(info, context);
		}

	    private SessionDO m_Session;
        public SessionDO Session {
			get {
				return m_Session;
			}
			set {
				m_Session = value;
			}
		}
	}
}