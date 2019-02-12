using Microsoft.EntityFrameworkCore;
using Pregledi.Domain.Entities;
using Pregledi.Domain.Repositories;
using Pregledi.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pregledi.Persistence.Repositories
{
	public class KarticaKontaRepository : IKarticaKontaRepository
	{
		private readonly PreglediContext context;

		public KarticaKontaRepository(PreglediContext context)
		{
			this.context = context;
		}

		public async Task<KarticaKonta> GetAsync(Guid id)
		{
			return await context.KarticaKonta.FindAsync(id);
		}

		public void Add(KarticaKonta stavka)
		{
			context.KarticaKonta.Add(stavka);
		}

		public void Remove(KarticaKonta stavka)
		{
			context.KarticaKonta.Remove(stavka);
		}

		public async Task<IList<KarticaKonta>> GetStavkeNalogaAsync(Guid idNaloga)
		{
			return await context.KarticaKonta.Where(x => x.IdNaloga == idNaloga)
				.OrderBy(x => x.DatumNaloga)
				.ThenBy(x => x.DatumKnjizenja)
				.ThenBy(x => x.Created)
				.ToListAsync();
		}

		public async Task<IList<KarticaKonta>> GetAllAsync(long idKonto)
		{
			return await context.KarticaKonta.Where(x => x.IdKonto == idKonto)
				.OrderBy(x => x.DatumNaloga)
				.ThenBy(x => x.DatumKnjizenja)
				.ThenBy(x => x.Created)
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task IzracunajAsync()
		{
			await context.Database.ExecuteSqlCommandAsync("CALL izracunaj_karticu_konta");
		}
	}
}
