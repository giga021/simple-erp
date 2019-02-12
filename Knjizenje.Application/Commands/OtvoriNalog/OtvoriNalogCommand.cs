using Knjizenje.Application.Commands.DTO;
using System;
using System.Collections.Generic;

namespace Knjizenje.Application.Commands.OtvoriNalog
{
	public class OtvoriNalogCommand : ICommand
	{
		public Guid CommandId { get; }
		public long? Version { get; }
		public string UserId { get; }
		public long IdTip { get; }
		public DateTime DatumNaloga { get; }
		public string Opis { get; }
		public List<StavkaDTO> Stavke { get; }

		public OtvoriNalogCommand(Guid commandId, string userId, long idTip, DateTime datumNaloga,
			string opis, IEnumerable<StavkaDTO> stavke)
		{
			this.CommandId = commandId;
			this.Version = null;
			this.UserId = userId;
			this.IdTip = idTip;
			this.DatumNaloga = datumNaloga;
			this.Opis = opis;
			this.Stavke = new List<StavkaDTO>();
			this.Stavke.AddRange(stavke);
		}
	}
}
