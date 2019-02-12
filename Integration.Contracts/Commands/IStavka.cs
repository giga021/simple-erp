using System;

namespace Integration.Contracts.Commands
{
	public interface IStavka
	{
		Guid? IdStavke { get; }
		long IdKonto { get; }
		decimal Duguje { get; }
		decimal Potrazuje { get; }
		string Opis { get; }
		bool Stornirana { get; }
	}
}
