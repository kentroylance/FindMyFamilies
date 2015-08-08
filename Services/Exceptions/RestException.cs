using System;
using System.Runtime.Serialization;
using FindMyFamilies.Data;

namespace FindMyFamilies.Exceptions {
	/// <summary>
	/// Summary description for DataAccessException.
	/// </summary>
	public class RestException : BaseException {
        
		public RestException() {
		}

		public RestException(string message) : base(message) {
		}

		public RestException(string message, Exception e) : base(message, e) {
		}

		public RestException(string message, Exception e, object logCategory, DataTransferObject dataTransferObject) : base(message, e, logCategory, dataTransferObject) {
		}

		public RestException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context) {
			base.GetObjectData(info, context);
		}

        public String DiscoveryPath { get; set; }

	}

}