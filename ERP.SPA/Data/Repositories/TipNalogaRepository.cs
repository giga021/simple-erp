using ERP.SPA.Data.EntityFramework;
using ERP.SPA.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.SPA.Data.Repositories
{
	public interface ITipNalogaRepository
	{
		Task<IList<TipNaloga>> GetAllAsync();
	}

	public class TipNalogaRepository : ITipNalogaRepository
	{
		private readonly WebContext context;

		public TipNalogaRepository(WebContext context)
		{
			this.context = context;
		}

		public async Task<IList<TipNaloga>> GetAllAsync()
		{
			return await context.TipoviNaloga.ToListAsync();
		}
	}
}
