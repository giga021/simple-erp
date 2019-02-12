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
	public class NalogGKRepository : INalogGKRepository
	{
		private readonly PreglediContext context;

		public NalogGKRepository(PreglediContext context)
		{
			this.context = context;
		}

		public async Task<NalogGlavnaKnjiga> GetAsync(Guid id)
		{
			return await context.NaloziGlavneKnjige.FindAsync(id);
		}

		public void Add(NalogGlavnaKnjiga nalog)
		{
			context.NaloziGlavneKnjige.Add(nalog);
		}

		public async Task<IList<NalogGlavnaKnjiga>> GetAllAsync()
		{
			return await context.NaloziGlavneKnjige
				.OrderByDescending(x => x.Datum)
				.ThenBy(x => x.TipNaziv)
				.ThenBy(x => x.Id)
				.AsNoTracking()
				.ToListAsync();
		}

		public void Remove(NalogGlavnaKnjiga nalog)
		{
			context.NaloziGlavneKnjige.Remove(nalog);
		}
	}
}
