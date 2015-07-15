using System;
using System.Runtime.Serialization;
using FindMyFamilies.Data;

namespace FindMyFamilies.Exceptions {
	/// <summary>
	/// Summary description for ApplicationException.
	/// </summary>
	[Serializable]
	public class SecurityException : BaseException {
		public SecurityException() : base() {
		}

		public SecurityException(string message) : base(message) {
		}

		public SecurityException(string message, Exception e) : base(message, e) {
		}

		public SecurityException(string message, Exception e, object logCategory) : base(message, e, logCategory) {
		}

		public SecurityException(string message, Exception e, object logCategory, DataTransferObject dataTransferObject) : base(message, e, logCategory, dataTransferObject) {
		}

		public SecurityException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context) {
			base.GetObjectData(info, context);
		}
	}
}