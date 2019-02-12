using Knjizenje.Domain.Entities.FinNalogAggregate;
using Knjizenje.Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Knjizenje.Domain.Events
{
	public class StavkaProknjizena : EventBase
	{
		public FinNalogId IdNaloga { get; set; }
		public Guid IdStavke { get; set; }
		public long IdKonto { get; private set; }
		public decimal Duguje { get; private set; }
		public decimal Potrazuje { get; private set; }
		public DateTime DatumKnjizenja { get; private set; }
		public string Opis { get; private set; }

		public StavkaProknjizena(FinNalogId idNaloga, Guid idStavke, DateTime datumKnjizenja, 
			long idKonto, decimal duguje, decimal potrazuje, string opis)
		{
			this.IdNaloga = idNaloga;
			this.IdStavke = idStavke;
			this.IdKonto = idKonto;
			this.Duguje = duguje;
			this.Potrazuje = potrazuje;
			this.DatumKnjizenja = datumKnjizenja;
			this.Opis = opis;
		}
	}
}
