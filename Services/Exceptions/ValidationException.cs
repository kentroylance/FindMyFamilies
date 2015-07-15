using System;
using System.Runtime.Serialization;
using FindMyFamilies.Data;

namespace FindMyFamilies.Exceptions {
	/// <summary>
	/// Summary description for ApplicationException.
	/// </summary>
	[Serializable]
	public class ValidationException : BaseException {
		public ValidationException() : base() {
		}

		public ValidationException(string message) : base(message) {
		}

		public ValidationException(string message, Exception e) : base(message, e) {
		}

		public ValidationException(string message, Exception e, object logCategory) : base(message, e, logCategory) {
		}

		public ValidationException(string message, Exception e, object logCategory, DataTransferObject dataTransferObject) : base(message, e, logCategory, dataTransferObject) {
		}

		public ValidationException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context) {
			base.GetObjectData(info, context);
		}
	}
}