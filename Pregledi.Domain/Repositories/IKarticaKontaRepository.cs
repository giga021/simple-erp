using Pregledi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pregledi.Domain.Repositories
{
	public interface IKarticaKontaRepository
	{
		void Add(KarticaKonta stavka);
		Task<KarticaKonta> GetAsync(Guid id);
		Task<IList<KarticaKonta>> GetAllAsync(long idKonto);
		Task<IList<KarticaKonta>> GetStavkeNalogaAsync(Guid idNaloga);
		void Remove(KarticaKonta stavka);
		Task IzracunajAsync();
	}
}
