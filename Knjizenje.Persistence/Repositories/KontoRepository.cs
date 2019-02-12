using Knjizenje.Domain.Entities.Konto;
using Knjizenje.Domain.Seedwork;
using Knjizenje.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Knjizenje.Persistence.Repositories
{
	public class KontoRepository : IKontoRepository
	{
		private readonly KnjizenjeContext context;
		public IUnitOfWork UnitOfWork => context;

		public KontoRepository(KnjizenjeContext context)
		{
			this.context = context;
		}

		public async Task<Konto> GetBySifraAsync(string sifra)
		{
			if (sifra == null)
				throw new ArgumentNullException(nameof(sifra));

			return await context.Konta
				.Where(x => x.Sifra == sifra)
				.SingleOrDefaultAsync();
		}

		public async Task<Dictionary<string, long>> GetIdBySifraAsync(params string[] sifra)
		{
			if (sifra == null)
				throw new ArgumentNullException(nameof(sifra));

			return await context.Konta
				.Where(x => sifra.Contains(x.Sifra))
				.ToDictionaryAsync(x => x.Sifra, x => x.Id);
		}
	}
}
