using ERP.SPA.Data.EntityFramework;
using ERP.SPA.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.SPA.Data.Repositories
{
	public interface IKontoRepository
	{
		Task<IList<Konto>> GetAllAsync();
	}

	public class KontoRepository : IKontoRepository
	{
		private readonly WebContext context;

		public KontoRepository(WebContext context)
		{
			this.context = context;
		}

		public async Task<IList<Konto>> GetAllAsync()
		{
			return await context.Konta.ToListAsync();
		}
	}
}
