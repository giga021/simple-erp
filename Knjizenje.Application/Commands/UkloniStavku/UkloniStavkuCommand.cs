using System;

namespace Knjizenje.Application.Commands.UkloniStavku
{
	public class UkloniStavkuCommand : ICommand
	{
		public Guid CommandId { get; }
		public long? Version { get; }
		public string UserId { get; }
		public Guid IdNaloga { get; }
		public Guid IdStavke { get; }

		public UkloniStavkuCommand(Guid commandId, long? version, string userId, Guid idNaloga, Guid idStavke)
		{
			this.CommandId = commandId;
			this.Version = version;
			this.UserId = userId;
			this.IdNaloga = idNaloga;
			this.IdStavke = idStavke;
		}
	}
}
