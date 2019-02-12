using Pregledi.Domain.Entities;
using Pregledi.Domain.Repositories;
using Pregledi.Persistence.Context;
using System.Threading.Tasks;

namespace Pregledi.Persistence.Repositories
{
	public class KontoRepository : IKontoRepository
	{
		private readonly PreglediContext context;

		public KontoRepository(PreglediContext context)
		{
			this.context = context;
		}

		public async Task<Konto> GetAsync(long id)
		{
			return await context.Konta.FindAsync(id);
		}
	}
}
