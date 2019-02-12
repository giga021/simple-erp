using ERP.SPA.Data.Repositories;
using ERP.SPA.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.SPA.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class KontaController : ControllerBase
	{
		private readonly IKontoRepository kontoRepo;

		public KontaController(IKontoRepository kontoRepo)
		{
			this.kontoRepo = kontoRepo;
		}

		[HttpGet]
		public async Task<IList<Konto>> Get()
		{
			return await kontoRepo.GetAllAsync();
		}
	}
}
