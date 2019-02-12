using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Knjizenje.Domain.Exceptions
{
    public class NalogNePostojiException : KnjizenjeException
    {
		public NalogNePostojiException(Guid idNaloga) : base($"Nalog {idNaloga} ne postoji") { }
    }
}
