using ERP.SPA.Data.Repositories;
using ERP.SPA.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.SPA.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TipoviNalogaController : ControllerBase
	{
		private readonly ITipNalogaRepository tipRepo;

		public TipoviNalogaController(ITipNalogaRepository tipRepo)
		{
			this.tipRepo = tipRepo;
		}

		[HttpGet]
		public async Task<IList<TipNaloga>> Get()
		{
			return await tipRepo.GetAllAsync();
		}
	}
}
