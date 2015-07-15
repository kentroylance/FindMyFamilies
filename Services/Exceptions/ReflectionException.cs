using System;
using System.Runtime.Serialization;
using FindMyFamilies.Data;

namespace FindMyFamilies.Exceptions {
	/// <summary>
	/// The is the base exception class for Patterns.NET
	/// </summary>
	public class ReflectionException : BaseException {
		/// <summary>
		/// Self explanation, shouldn't be used much
		/// </summary>
		public ReflectionException() : base() {
			;
		}

		/// <summary>
		/// Same as following except this is no code
		/// </summary>
		/// <param name="message"></param>
		public ReflectionException(string message) : base(message) {
			;
		}

		/// <summary>
		/// Allow inner exception to be created to create exception chaining
		/// </summary>
		/// <param name="message"></param>
		/// <param name="innerException"></param>
		public ReflectionException(string message, Exception innerException) : base(message, innerException) {
			;
		}

		/// <summary>
		/// Allow inner exception to be created to create exception chaining
		/// </summary>
		/// <param name="message"></param>
		/// <param name="innerException"></param>
		public ReflectionException(string message, Exception innerException, object logCategory) : base(message, innerException, logCategory) {
			;
		}

		/// <summary>
		/// Allow inner exception to be created to create exception chaining
		/// </summary>
		/// <param name="message"></param>
		/// <param name="innerException"></param>
		public ReflectionException(string message, Exception innerException, object logCategory, DataTransferObject dataTransferObject) : base(message, innerException, logCategory, dataTransferObject) {
			;
		}

		public ReflectionException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context) {
			base.GetObjectData(info, context);
		}

	}
}