using System;
using System.Runtime.Serialization;
using FindMyFamilies.Data;

namespace FindMyFamilies.Exceptions {
	/// <summary>
	/// Summary description for ApplicationException.
	/// </summary>
	[Serializable]
	public class ResourceException : BaseException {
		public ResourceException() : base() {
		}

		public ResourceException(string message) : base(message) {
		}

		public ResourceException(string message, Exception e) : base(message, e) {
		}

		public ResourceException(string message, Exception e, object logCategory) : base(message, e, logCategory) {
		}

		public ResourceException(string message, Exception e, object logCategory, DataTransferObject dataTransferObject) : base(message, e, logCategory, dataTransferObject) {
		}

		public ResourceException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context) {
			base.GetObjectData(info, context);
		}
	}
}