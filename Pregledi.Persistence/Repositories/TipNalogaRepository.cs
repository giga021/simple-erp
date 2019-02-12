using Pregledi.Domain.Entities;
using Pregledi.Domain.Repositories;
using Pregledi.Persistence.Context;
using System.Threading.Tasks;

namespace Pregledi.Persistence.Repositories
{
	public class TipNalogaRepository : ITipNalogaRepository
	{
		private readonly PreglediContext context;

		public TipNalogaRepository(PreglediContext context)
		{
			this.context = context;
		}

		public async Task<TipNaloga> GetAsync(long id)
		{
			return await context.TipoviNaloga.FindAsync(id);
		}
	}
}
