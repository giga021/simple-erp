using System;

namespace Pregledi.Application.Exceptions
{
	public class NalogNePostojiException : PreglediException
	{
		public NalogNePostojiException(Guid idNaloga) : base($"Nalog {idNaloga} ne postoji") { }
	}
}
