namespace Knjizenje.Domain.DTO
{
	public class StavkaIzvoda
	{
		public int SifraPlacanja { get; }
		public decimal Duguje { get; }
		public decimal Potrazuje { get; }

		public StavkaIzvoda(int sifraPlacanja, decimal duguje, decimal potrazuje)
		{
			this.SifraPlacanja = sifraPlacanja;
			this.Duguje = duguje;
			this.Potrazuje = potrazuje;
		}
	}
}
