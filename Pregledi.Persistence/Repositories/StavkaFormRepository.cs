using Pregledi.Domain.Entities;
using Pregledi.Domain.Repositories;
using Pregledi.Persistence.Context;
using System;
using System.Threading.Tasks;

namespace Pregledi.Persistence.Repositories
{
	public class StavkaFormRepository : IStavkaFormRepository
	{
		private readonly PreglediContext context;

		public StavkaFormRepository(PreglediContext context)
		{
			this.context = context;
		}

		public async Task<StavkaForm> GetAsync(Guid id)
		{
			return await context.StavkeForm.FindAsync(id);
		}

		public void Add(StavkaForm stavka)
		{
			context.StavkeForm.Add(stavka);
		}

		public void Remove(StavkaForm stavka)
		{
			context.StavkeForm.Remove(stavka);
		}
	}
}
