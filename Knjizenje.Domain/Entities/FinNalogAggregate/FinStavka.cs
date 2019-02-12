using Knjizenje.Domain.Seedwork;
using System;

namespace Knjizenje.Domain.Entities.FinNalogAggregate
{
	public class FinStavka : Entity<Guid>
	{
		public long IdKonto { get; }
		public DateTime DatumKnjizenja { get; }
		public IznosStavke Iznos { get; }
		public string Opis { get; }

		protected FinStavka() { }

		internal FinStavka(Guid id, long idKonto, decimal duguje, decimal potrazuje, string opis, DateTime datumKnjizenja)
		{
			this.Id = id;
			this.IdKonto = idKonto;
			this.Iznos = new IznosStavke(duguje, potrazuje);
			this.Opis = opis;
			this.DatumKnjizenja = datumKnjizenja;
		}

		public FinStavka(long idKonto, decimal duguje, decimal potrazuje, string opis)
			: this(Guid.NewGuid(), idKonto, duguje, potrazuje, opis, default) { }
	}
}
