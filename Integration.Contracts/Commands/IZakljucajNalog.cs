using System;

namespace Integration.Contracts.Commands
{
	public interface IZakljucajNalog : IBaseCommand
	{
		Guid IdNaloga { get; }
	}
}
