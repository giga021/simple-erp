using System;
using System.Collections.Generic;

namespace Knjizenje.Application.Commands.ProknjiziIzvod
{
	public class ProknjiziIzvodCommand : ICommand
	{
		public Guid CommandId { get; }
		public long? Version { get; }
		public string UserId { get; }
		public DateTime Datum { get; }
		public List<StavkaIzvodaDTO> Stavke { get; }

		public ProknjiziIzvodCommand(Guid commandId, long? version, string userId, DateTime datum, IEnumerable<StavkaIzvodaDTO> stavke)
		{
			this.CommandId = commandId;
			this.Version = version;
			this.UserId = userId;
			this.Datum = datum;
			this.Stavke = new List<StavkaIzvodaDTO>();
			this.Stavke.AddRange(stavke);
		}
	}
}
