using System;

namespace Integration.Contracts.Commands
{
	public interface IOtkljucajNalog : IBaseCommand
	{
		Guid IdNaloga { get; }
	}
}
