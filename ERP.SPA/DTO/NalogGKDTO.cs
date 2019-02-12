using System;

namespace ERP.SPA.DTO
{
	public class NalogGKDTO
	{
		public Guid Id { get; set; }
		public long Version { get; set; }
		public DateTime Datum { get; set; }
		public string TipNaziv { get; set; }
		public string Opis { get; set; }
		public int BrojStavki { get; set; }
		public decimal UkupnoDuguje { get; set; }
		public decimal UkupnoPotrazuje { get; set; }
		public bool Zakljucan { get; set; }
	}
}
