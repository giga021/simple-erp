using MediatR;
using System;

namespace Knjizenje.Application.Commands
{
	public interface ICommand : IRequest<bool>
	{
		Guid CommandId { get; }
		long? Version { get; }
		string UserId { get; }
	}
}
