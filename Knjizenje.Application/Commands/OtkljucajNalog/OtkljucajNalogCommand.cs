using System;

namespace Knjizenje.Application.Commands.OtkljucajNalog
{
	public class OtkljucajNalogCommand : ICommand
	{
		public Guid CommandId { get; }
		public long? Version { get; }
		public string UserId { get; }
		public Guid IdNaloga { get; }

		public OtkljucajNalogCommand(Guid commandId, long? version, string userId, Guid idNaloga)
		{
			this.CommandId = commandId;
			this.Version = version;
			this.UserId = userId;
			this.IdNaloga = idNaloga;
		}
	}
}
