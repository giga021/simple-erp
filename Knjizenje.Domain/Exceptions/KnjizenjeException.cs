using System;
using System.Collections.Generic;
using System.Text;

namespace Knjizenje.Domain.Exceptions
{
    public class KnjizenjeException : Exception
    {
		public KnjizenjeException() { }

		public KnjizenjeException(string message) : base(message) { }

		public KnjizenjeException(string message, Exception innerException)
			: base(message, innerException) { }
	}
}
