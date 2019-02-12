using System;
using System.Collections.Generic;

namespace Integration.Contracts.Commands
{
	public interface IProknjiziIzvod : IBaseCommand
	{
		DateTime Datum { get; }
		IEnumerable<IStavkaIzvoda> Stavke { get; }
	}
}
