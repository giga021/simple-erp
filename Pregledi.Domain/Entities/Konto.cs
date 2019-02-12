using Pregledi.Domain.Seedwork;

namespace Pregledi.Domain.Entities
{
	public class Konto : Entity<long>
	{
		public string Sifra { get; set; }
		public string Naziv { get; set; }
	}
}
