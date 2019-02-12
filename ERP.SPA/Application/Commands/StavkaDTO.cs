using Integration.Contracts.Commands;
using System;

namespace ERP.SPA.Application.Commands
{
	public class StavkaDTO : IStavka
	{
		public Guid? IdStavke { get; }
		public long IdKonto { get; }
		public decimal Duguje { get; }
		public decimal Potrazuje { get; }
		public string Opis { get; }
		public bool Stornirana { get; }

		public StavkaDTO(Guid? idStavke, long idKonto, decimal duguje, decimal potrazuje, string opis, bool stornirana)
		{
			this.IdStavke = idStavke;
			this.IdKonto = idKonto;
			this.Duguje = duguje;
			this.Potrazuje = potrazuje;
			this.Opis = opis;
			this.Stornirana = stornirana;
		}
	}
}
