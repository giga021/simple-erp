using System;

namespace Integration.Contracts.Commands
{
	public interface IBaseCommand
	{
		Guid CommandId { get; }
		long? Version { get; }
		string UserId { get; }
	}
}
