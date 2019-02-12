using System;
using System.Collections.Generic;

namespace Integration.Contracts.Commands
{
	public interface IOtvoriNalog : IBaseCommand
	{
		long IdTip { get; }
		DateTime DatumNaloga { get; }
		string Opis { get; }
		IEnumerable<IStavka> Stavke { get; }
	}
}
