using System;

namespace Integration.Contracts.Commands
{
	public interface IUkloniStavku : IBaseCommand
	{
		Guid IdNaloga { get; }
		Guid IdStavke { get; }
	}
}
