using System;
using System.Runtime.Serialization;
using FindMyFamilies.Data;

namespace FindMyFamilies.Exceptions {
	/// <summary>
	/// Summary description for ApplicationException.
	/// </summary>
	public class RequestException : BaseException {
		public RequestException() : base() {
		}

		public RequestException(string message) : base(message) {
		}

		public RequestException(string message, Exception e) : base(message, e) {
		}

		public RequestException(string message, Exception e, object logCategory) : base(message, e, logCategory) {
		}

		public RequestException(string message, Exception e, object logCategory, DataTransferObject dataTransferObject) : base(message, e, logCategory, dataTransferObject) {
		}

		public RequestException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context) {
			base.GetObjectData(info, context);
		}
	}
}