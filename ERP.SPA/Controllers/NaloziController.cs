using ERP.SPA.Application.Services;
using ERP.SPA.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ERP.SPA.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class NaloziController : ControllerBase
	{
		private readonly IPreglediService preglediSvc;
		private readonly IKnjizenjeService knjizenjeSvc;

		public NaloziController(IPreglediService preglediSvc, IKnjizenjeService knjizenjeSvc)
		{
			this.preglediSvc = preglediSvc;
			this.knjizenjeSvc = knjizenjeSvc;
		}

		[HttpPost]
		public async Task<ActionResult> Post([FromBody] NalogFormDTO nalog)
		{
			await knjizenjeSvc.SnimiNoviAsync(nalog);
			return Ok();
		}

		[HttpPut]
		public async Task<ActionResult> Put([FromBody] NalogFormDTO nalog)
		{
			await knjizenjeSvc.SnimiPromenuAsync(nalog);
			return Ok();
		}

		[HttpPut("{id}/zakljucaj")]
		public async Task<ActionResult> Zakljucaj(Guid id, long version)
		{
			await knjizenjeSvc.ZakljucajAsync(id, version);
			return Ok();
		}

		[HttpPut("{id}/otkljucaj")]
		public async Task<ActionResult> Otkljucaj(Guid id, long version)
		{
			await knjizenjeSvc.OtkljucajAsync(id, version);
			return Ok();
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> Obrisi(Guid id, long version)
		{
			await knjizenjeSvc.ObrisiAsync(id, version);
			return Ok();
		}

		[HttpGet("promena/{id}")]
		public async Task<ActionResult<NalogFormDTO>> GetPromena(Guid id)
		{
			return Ok(await preglediSvc.GetNalogFormAsync(id));
		}
	}
}
