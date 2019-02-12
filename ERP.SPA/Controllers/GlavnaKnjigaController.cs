using ERP.SPA.Application.Services;
using ERP.SPA.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.SPA.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class GlavnaKnjigaController : ControllerBase
	{
		private readonly IPreglediService preglediSvc;

		public GlavnaKnjigaController(IPreglediService preglediSvc)
		{
			this.preglediSvc = preglediSvc;
		}

		[HttpGet]
		public async Task<ActionResult<IList<NalogGKDTO>>> Index()
		{
			return Ok(await preglediSvc.GetGlavnaKnjigaAsync());
		}
	}
}
