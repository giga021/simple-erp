using Integration.Contracts.Commands;
using System;
using System.Collections.Generic;

namespace ERP.SPA.Application.Commands
{
	public class IzmeniNalog : IIzmeniNalog
	{
		public Guid CommandId { get; }
		public long? Version { get; }
		public string UserId { get; }
		public Guid IdNaloga { get; }
		public long IdTip { get; }
		public DateTime DatumNaloga { get; }
		public string Opis { get; }
		public IEnumerable<IStavka> Stavke { get; }

		public IzmeniNalog(Guid commandId, long? version, string userId, Guid idNaloga,
			long idTip, DateTime datumNaloga, string opis, IEnumerable<StavkaDTO> stavke)
		{
			this.CommandId = commandId;
			this.Version = version;
			this.UserId = userId;
			this.IdNaloga = idNaloga;
			this.IdTip = idTip;
			this.DatumNaloga = datumNaloga;
			this.Opis = opis;
			this.Stavke = stavke;
		}
	}
}
