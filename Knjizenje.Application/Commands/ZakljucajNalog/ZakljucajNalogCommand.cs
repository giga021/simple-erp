using System;

namespace Knjizenje.Application.Commands.ZakljucajNalog
{
	public class ZakljucajNalogCommand : ICommand
	{
		public Guid CommandId { get; }
		public long? Version { get; }
		public string UserId { get; }
		public Guid IdNaloga { get; }

		public ZakljucajNalogCommand(Guid commandId, long? version, string userId, Guid idNaloga)
		{
			this.CommandId = commandId;
			this.Version = version;
			this.UserId = userId;
			this.IdNaloga = idNaloga;
		}
	}
}
