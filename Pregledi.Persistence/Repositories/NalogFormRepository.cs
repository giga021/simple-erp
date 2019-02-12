using Microsoft.EntityFrameworkCore;
using Pregledi.Domain.Entities;
using Pregledi.Domain.Repositories;
using Pregledi.Persistence.Context;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pregledi.Persistence.Repositories
{
	public class NalogFormRepository : INalogFormRepository
	{
		private readonly PreglediContext context;

		public NalogFormRepository(PreglediContext context)
		{
			this.context = context;
		}

		public async Task<NalogForm> GetAsync(Guid id)
		{
			return await context.NaloziForm.FindAsync(id);
		}

		public void Add(NalogForm nalog)
		{
			context.NaloziForm.Add(nalog);
		}

		public async Task<NalogForm> GetSaStavkamaAsync(Guid id)
		{
			return await context.NaloziForm.Where(x => x.Id == id)
				.Include(x => x.Stavke).SingleOrDefaultAsync();
		}

		public void Remove(NalogForm nalog)
		{
			context.NaloziForm.Remove(nalog);
		}
	}
}
