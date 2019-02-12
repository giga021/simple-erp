using ERP.SPA.Application.Services;
using ERP.SPA.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.SPA.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class KarticaKontaController : ControllerBase
	{
		private readonly IPreglediService preglediSvc;

		public KarticaKontaController(IPreglediService preglediSvc)
		{
			this.preglediSvc = preglediSvc;
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<IList<KarticaKontaDTO>>> GetKarticaKonta(long id)
		{
			return Ok(await preglediSvc.GetKarticaKontaAsync(id));
		}
	}
}
