using System;

namespace Integration.Contracts.Commands
{
	public interface IObrisiNalog : IBaseCommand
	{
		Guid IdNaloga { get; }
	}
}
