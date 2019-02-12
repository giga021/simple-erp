using Knjizenje.Domain.DTO;
using Knjizenje.Domain.Entities.FinNalogAggregate;
using Knjizenje.Domain.Entities.Konto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Knjizenje.Domain.Services
{
	public interface IIzvodService
	{
		Task<IList<FinStavka>> FormirajStavkeNalogaAsync(IEnumerable<StavkaIzvoda> stavkeIzvoda);
	}

	public class IzvodService : IIzvodService
	{
		private readonly IKontoRepository kontoRepo;

		public IzvodService(IKontoRepository kontoRepo)
		{
			this.kontoRepo = kontoRepo;
		}

		public async Task<IList<FinStavka>> FormirajStavkeNalogaAsync(IEnumerable<StavkaIzvoda> stavkeIzvoda)
		{
			if (stavkeIzvoda == null)
				throw new ArgumentNullException(nameof(stavkeIzvoda));
			if (!stavkeIzvoda.Any())
				return new List<FinStavka>();

			string konto = "435";
			string kontraKonto = "241";
			var konta = await kontoRepo.GetIdBySifraAsync(konto, kontraKonto);

			var stavke = stavkeIzvoda.Select(x =>
			{
				var stavka = new FinStavka(konta[konto], x.Duguje, x.Potrazuje, null);
				var kontraStavka = new FinStavka(konta[kontraKonto], x.Potrazuje, x.Duguje, null);
				return new[] { stavka, kontraStavka };
			}).SelectMany(x => x).ToList();

			return stavke;
		}
	}
}
