using System;

namespace Integration.Contracts.Commands
{
	public interface IStornirajStavku : IBaseCommand
	{
		Guid IdNaloga { get; }
		Guid IdStavke { get; }
	}
}
