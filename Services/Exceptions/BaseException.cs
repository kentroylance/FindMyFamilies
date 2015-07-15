using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using FindMyFamilies.Data;
using FindMyFamilies.Util;

namespace FindMyFamilies.Exceptions {
	/// <summary>
	/// The is the base exception class for Patterns.NET
	/// </summary>
	/// 
	public class BaseException : System.ApplicationException {

	    private Logger logger = new Logger(MethodBase.GetCurrentMethod().DeclaringType);

		/// <summary>
		/// Self explanation, shouldn't be used much
		/// </summary>
		private bool m_isLogged;

		public BaseException() {
		}

		/// <summary>
		/// Same as following ctor except this is no code
		/// </summary>
		/// <param name="message"></param>
		public BaseException(string message) : this(message, null) {
		}

		/// <summary>
		/// Allow inner exception to be created to create exception chaining
		/// </summary>
		/// <param name="message"></param>
		/// <param name="innerException"></param>
		public BaseException(string message, Exception exception) : base(message, exception) {
			//log.Error(message, innerException);
			logger.Error(message, exception);

			//          if (innerException == null) {
			//              Logger.Error(message, null, innerException, null);
			//          } else {
			//              Logger.Error(this.e.buildErrorStackBuilder(innerException), null, innerException, null);
			//          }
			this.IsLogged = true;
		}

		/// <summary>
		/// Allow inner exception to be created to create exception chaining
		/// </summary>
		/// <param name="message"></param>
		/// <param name="innerException"></param>
		public BaseException(string message, Exception exception, object logCategory) : base(message, exception) {
			if (exception == null) {
    			logger.Error(message, exception);
//				Logger.Error(message, logCategory, this, null);
			} else {
       			logger.Error(message, exception);
//				Logger.Error(message, logCategory, exception, null);
			}
			this.IsLogged = true;
		}

		/// <summary>
		/// Allow inner exception to be created to create exception chaining
		/// </summary>
		/// <param name="message"></param>
		/// <param name="innerException"></param>
		public BaseException(string message, Exception exception, object logCategory, DataTransferObject dataTransferObject) : base(message, exception) {
			if (exception == null) {
       			logger.Error(message, exception);
//                Logger.Error(message, logCategory, this, dataTransferObject);
			} else {
    			logger.Error(message, exception);
//				Logger.Error(message, logCategory, exception, dataTransferObject);
			}
			this.IsLogged = true;
		}

		public static string GetAllMessages(string message, Exception ex) {
			for (Exception exception = ex; exception != null; exception = exception.InnerException) {
				message += exception.Message.Trim();
			}
			return message;						
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="source"></param>
		/// <param name="code"></param>
		/// <param name="message"></param>
		/// <param name="innerException"></param>
		/// <returns></returns>
		public string Format(string message, Exception innerException) {
			StringBuilder newmessage = new StringBuilder();
			string errorStackBuilder = null;

			// get the error stack, if innerException is null, errorStackBuilder will be "exception was not chained" and should never be null
			errorStackBuilder = this.BuildErrorStackBuilder(innerException);

			// we want immediate gradification 
			Trace.AutoFlush = true;

			newmessage.Append("Exception Summary \n")
				.Append("--------------------------------------------\n")
				.Append(DateTime.Now.ToShortDateString())
				.Append(":")
				.Append(DateTime.Now.ToShortTimeString())
				.Append(" - ")
				.Append(message)
				.Append("\n\n")
				.Append(errorStackBuilder);

			return newmessage.ToString();
		}

		public string MethodName {
			get {
				if (this.TargetSite != null) {
					return this.TargetSite.Name;
				} else {
					return "";
				}
			}
		}

		public string ApplicationName {
			get {
				if (this.Source != null) {
					return this.Source;
				} else {
					return "";
				}
			}
		}

		/// <summary>
		/// Takes a first nested exception object and builds a error stack from its chained contents
		/// </summary>
		/// <param name="chainedException"></param>
		/// <returns></returns>
		public string BuildErrorStackBuilder(Exception chainedException) {
			string errorStack = null;
			StringBuilder errorStackBuilder = new StringBuilder();
			int errStackNum = 1;
			Exception innerException = null;
			string appName = null;
			string methodName = null;

			if (chainedException != null) {
				errorStackBuilder.Append(this.Message + "\n");
				errorStackBuilder.Append("Exception Details (Exception Details --- Error Stack\n")
					.Append("-------------------------------------------\n");

				innerException = chainedException;

				while (innerException != null) {
					methodName = (innerException.TargetSite != null) ? innerException.TargetSite.Name : "<not set>";
					appName = (innerException.Source != null) ? innerException.Source : "<not set>";
					errorStackBuilder.Append(errStackNum)
						.Append(") ")
						.Append("Method: ")
						.Append(appName)
						.Append("\n    Assembly: ")
						.Append(methodName)
						.Append("\n    message: \n    ")
						.Append(innerException.Message)
						.Append("\n\n");

					innerException = innerException.InnerException;
					errStackNum++;
				}

				errorStackBuilder.Append("\nCall Stack\n")
					.Append("-------------------------------------------\n")
					.Append(chainedException.StackTrace);
			} else {
				errorStackBuilder.Append("exception was not chained");
			}

			return errorStackBuilder.ToString();
		}

		public bool IsLogged {
			get {
				return m_isLogged;
			}
			set {
				m_isLogged = value;
			}
		}

		public BaseException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context) {
			base.GetObjectData(info, context);
		}
	}
}