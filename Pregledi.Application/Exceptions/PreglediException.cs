using System;

namespace Pregledi.Application.Exceptions
{
	public class PreglediException : Exception
	{
		public PreglediException() { }

		public PreglediException(string message) : base(message) { }

		public PreglediException(string message, Exception innerException)
			: base(message, innerException) { }
	}
}
