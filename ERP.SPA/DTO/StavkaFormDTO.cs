using System;

namespace ERP.SPA.DTO
{
	public class StavkaFormDTO
	{
		public Guid? Id { get; set; }
		public long IdKonto { get; set; }
		public string Konto { get; set; }
		public decimal Duguje { get; set; }
		public decimal Potrazuje { get; set; }
		public string Opis { get; set; }
		public bool Stornirana { get; set; }
	}
}
