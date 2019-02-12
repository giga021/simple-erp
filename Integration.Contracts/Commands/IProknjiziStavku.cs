using System;

namespace Integration.Contracts.Commands
{
	public interface IProknjiziStavku : IBaseCommand
	{
		Guid IdNaloga { get; }
		long IdKonto { get; }
		decimal Duguje { get; }
		decimal Potrazuje { get; }
		string Opis { get; }
	}
}
