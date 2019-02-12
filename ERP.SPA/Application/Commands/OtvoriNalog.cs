using Integration.Contracts.Commands;
using System;
using System.Collections.Generic;

namespace ERP.SPA.Application.Commands
{
	public class OtvoriNalog : IOtvoriNalog
	{
		public Guid CommandId { get; }
		public long? Version { get; }
		public string UserId { get; }
		public long IdTip { get; }
		public DateTime DatumNaloga { get; }
		public string Opis { get; }
		public IEnumerable<IStavka> Stavke { get; }

		public OtvoriNalog(Guid commandId, string userId, long idTip, DateTime datumNaloga,
			string opis, IEnumerable<StavkaDTO> stavke)
		{
			this.CommandId = commandId;
			this.Version = null;
			this.UserId = userId;
			this.IdTip = idTip;
			this.DatumNaloga = datumNaloga;
			this.Opis = opis;
			this.Stavke = stavke;
		}
	}
}
