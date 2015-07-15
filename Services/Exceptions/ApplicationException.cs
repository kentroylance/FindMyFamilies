using System;
using System.Runtime.Serialization;
using FindMyFamilies.Data;

namespace FindMyFamilies.Exceptions {
	/// <summary>
	/// The is the base exception class for Patterns.NET
	/// </summary>
	[Serializable]
	public class ApplicationException : BaseException {
		/// <summary>
		/// Self explanation, shouldn't be used much
		/// </summary>
		public ApplicationException() : base() {
			;
		}

		/// <summary>
		/// Same as following except this is no code
		/// </summary>
		/// <param name="message"></param>
		public ApplicationException(string message) : base(message) {
			;
		}

		/// <summary>
		/// Allow inner exception to be created to create exception chaining
		/// </summary>
		/// <param name="message"></param>
		/// <param name="innerException"></param>
		public ApplicationException(string message, Exception innerException) : base(message, innerException) {
			;
		}

		/// <summary>
		/// Allow inner exception to be created to create exception chaining
		/// </summary>
		/// <param name="message"></param>
		/// <param name="innerException"></param>
		public ApplicationException(string message, Exception innerException, object logCategory) : base(message, innerException, logCategory) {
			;
		}

		/// <summary>
		/// Allow inner exception to be created to create exception chaining
		/// </summary>
		/// <param name="message"></param>
		/// <param name="innerException"></param>
		public ApplicationException(string message, Exception innerException, object logCategory, DataTransferObject dataTransferObject) : base(message, innerException, logCategory, dataTransferObject) {
			;
		}

		public ApplicationException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context) {
			base.GetObjectData(info, context);
		}

	}
}