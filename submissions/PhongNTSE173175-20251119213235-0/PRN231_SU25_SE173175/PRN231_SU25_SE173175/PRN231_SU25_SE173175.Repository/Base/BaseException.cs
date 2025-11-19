using System.Net;

namespace PRN231_SU25_SE173175.Repository.Base
{
	public class BaseException : Exception
	{
		public int StatusCode { get; set; }
		public object? ErrorMessage { get; set; }
		public BaseException(object statusCode, string message)
			: base(message)
		{
			ErrorMessage = message;
			if (statusCode is int code)
				StatusCode = code;
			else if (statusCode is string s && int.TryParse(s, out var parsed))
				StatusCode = parsed;
			else
				StatusCode = 400;
		}
	}
}
