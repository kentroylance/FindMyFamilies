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

		public override void GetObjectData(SerializationInfo info, StreamingContext context) {
			base.GetObjectData(info, context);
		}
	}
}