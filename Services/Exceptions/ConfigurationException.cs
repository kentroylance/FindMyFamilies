using System;
using System.Runtime.Serialization;
using FindMyFamilies.Data;

namespace FindMyFamilies.Exceptions {
	/// <summary>
	/// Summary description for ConfigurationException.
	/// </summary>
	public class ConfigurationException : BaseException {
		public ConfigurationException() : base() {
		}

		public ConfigurationException(string message) : base(message) {
		}

		public ConfigurationException(string message, Exception e) : base(message, e) {
		}

		public ConfigurationException(string message, Exception e, object logCategory) : base(message, e, logCategory) {
		}

		public ConfigurationException(string message, Exception e, object logCategory, DataTransferObject dataTransferObject) : base(message, e, logCategory, dataTransferObject) {
		}

		public ConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context) {
			base.GetObjectData(info, context);
		}

	}
}