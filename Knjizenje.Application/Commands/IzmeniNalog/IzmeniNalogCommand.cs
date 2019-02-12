using Knjizenje.Application.Commands.DTO;
using System;
using System.Collections.Generic;

namespace Knjizenje.Application.Commands.IzmeniNalog
{
	public class IzmeniNalogCommand : ICommand
	{
		public Guid CommandId { get; }
		public long? Version { get; }
		public string UserId { get; }
		public Guid IdNaloga { get; }
		public long IdTip { get; }
		public DateTime DatumNaloga { get; }
		public string Opis { get; }
		public IEnumerable<StavkaDTO> Stavke { get; }

		public IzmeniNalogCommand(Guid commandId, long? version, string userId, Guid idNaloga,
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
