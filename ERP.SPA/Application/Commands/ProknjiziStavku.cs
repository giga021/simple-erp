using Integration.Contracts.Commands;
using System;

namespace ERP.SPA.Application.Commands
{
	public class ProknjiziStavku : IProknjiziStavku
	{
		public Guid CommandId { get; }
		public long? Version { get; }
		public string UserId { get; }
		public Guid IdNaloga { get; }
		public long IdKonto { get; }
		public decimal Duguje { get; }
		public decimal Potrazuje { get; }
		public string Opis { get; }

		public ProknjiziStavku(Guid commandId, long? version, string userId, Guid idNaloga, long idKonto, decimal duguje, decimal potrazuje,
			string opis)
		{
			this.CommandId = commandId;
			this.Version = version;
			this.UserId = userId;
			this.IdNaloga = idNaloga;
			this.IdKonto = idKonto;
			this.Duguje = duguje;
			this.Potrazuje = potrazuje;
			this.Opis = opis;
		}
	}
}
