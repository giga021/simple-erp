using System;

namespace ERP.SPA.DTO
{
	public class KarticaKontaDTO
	{
		public Guid Id { get; set; }
		public DateTime DatumNaloga { get; set; }
		public string TipNaloga { get; set; }
		public DateTime DatumKnjizenja { get; set; }
		public long IdKonto { get; set; }
		public string Konto { get; set; }
		public string Opis { get; set; }
		public decimal Duguje { get; set; }
		public decimal Potrazuje { get; set; }
		public decimal Saldo { get; set; }
		public decimal SaldoKumulativno { get; set; }
	}
}
