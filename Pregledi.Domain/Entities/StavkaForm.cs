using Pregledi.Domain.Seedwork;
using System;

namespace Pregledi.Domain.Entities
{
	public class StavkaForm : Entity<Guid>
	{
		public Guid IdNaloga { get; set; }
		public long IdKonto { get; set; }
		public string Konto { get; set; }
		public DateTime DatumKnjizenja { get; set; }
		public decimal Duguje { get; set; }
		public decimal Potrazuje { get; set; }
		public string Opis { get; set; }
		public long Version { get; set; }
	}
}
