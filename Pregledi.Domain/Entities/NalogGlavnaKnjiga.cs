using Pregledi.Domain.Seedwork;
using System;

namespace Pregledi.Domain.Entities
{
	public class NalogGlavnaKnjiga : Entity<Guid>
	{
		public DateTime Datum { get; set; }
		public string TipNaziv { get; set; }
		public string Opis { get; set; }
		public int BrojStavki { get; set; }
		public decimal UkupnoDuguje { get; set; }
		public decimal UkupnoPotrazuje { get; set; }
		public decimal UkupnoSaldo { get; set; }
		public bool Zakljucan { get; set; }
		public long Version { get; set; }
	}
}
