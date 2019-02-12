using System;

namespace Knjizenje.Application.Commands.StornirajStavku
{
	public class StornirajStavkuCommand : ICommand
	{
		public Guid CommandId { get; }
		public long? Version { get; }
		public string UserId { get; }
		public Guid IdNaloga { get; }
		public Guid IdStavke { get; }

		public StornirajStavkuCommand(Guid commandId, long? version, string userId, Guid idNaloga, Guid idStavke)
		{
			this.CommandId = commandId;
			this.Version = version;
			this.UserId = userId;
			this.IdNaloga = idNaloga;
			this.IdStavke = idStavke;
		}
	}
}
