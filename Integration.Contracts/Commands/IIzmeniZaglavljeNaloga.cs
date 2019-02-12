using System;

namespace Integration.Contracts.Commands
{
	public interface IIzmeniZaglavljeNaloga : IBaseCommand
	{
		Guid IdNaloga { get; }
		long IdTip { get; }
		DateTime DatumNaloga { get; }
		string Opis { get; }
	}
}
