using System.Net;

namespace FindMyFamilies.Util
{
	public class HttpException : System.Exception
	{
		private readonly HttpStatusCode statusCode;

		public HttpException (HttpStatusCode statusCode)
		{
			this.statusCode = statusCode;
		}

		public HttpStatusCode StatusCode {
			get {
				return this.statusCode;
			}
		}
	}

	public class ApiNonConformanceException : System.Exception
	{
		public ApiNonConformanceException(string message) : base(message)
		{

		}
	}
}

