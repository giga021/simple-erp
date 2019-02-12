using Knjizenje.Domain.Seedwork;

namespace Knjizenje.Domain.Entities.Konto
{
	public class Konto : Entity<long>
	{
		public string Sifra { get; private set; }
		public string Naziv { get; private set; }

		private Konto() { }

		public Konto(string sifra, string naziv)
		{
			this.Sifra = sifra;
			this.Naziv = naziv;
		}
	}
}
