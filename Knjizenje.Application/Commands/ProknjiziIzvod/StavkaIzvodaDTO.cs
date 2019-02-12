namespace Knjizenje.Application.Commands.ProknjiziIzvod
{
	public class StavkaIzvodaDTO
	{
		public int SifraPlacanja { get; }
		public decimal Duguje { get; }
		public decimal Potrazuje { get; }

		public StavkaIzvodaDTO(int sifraPlacanja, decimal duguje, decimal potrazuje)
		{
			this.SifraPlacanja = sifraPlacanja;
			this.Duguje = duguje;
			this.Potrazuje = potrazuje;
		}
	}
}
