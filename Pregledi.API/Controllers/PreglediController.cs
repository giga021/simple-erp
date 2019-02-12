using Microsoft.AspNetCore.Mvc;
using Pregledi.Domain.Entities;
using Pregledi.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pregledi.API.Controllers
{
	[Route("api")]
	[ApiController]
	public class PreglediController : ControllerBase
	{
		private readonly INalogGKRepository nalogGKRepo;
		private readonly IStavkaFormRepository stavkaFormRepo;
		private readonly INalogFormRepository nalogFormRepo;
		private readonly IKarticaKontaRepository karticaKontaRepo;

		public PreglediController(INalogGKRepository nalogGKRepo,
			IStavkaFormRepository stavkaFormRepo, INalogFormRepository nalogFormRepo,
			IKarticaKontaRepository karticaKontaRepo)
		{
			this.nalogGKRepo = nalogGKRepo;
			this.stavkaFormRepo = stavkaFormRepo;
			this.nalogFormRepo = nalogFormRepo;
			this.karticaKontaRepo = karticaKontaRepo;
		}

		[HttpGet("glavna-knjiga")]
		public async Task<ActionResult<IList<NalogGlavnaKnjiga>>> GetGlavnaKnjiga()
		{
			return Ok(await nalogGKRepo.GetAllAsync());
		}

		[HttpGet("nalog-form/{id}")]
		public async Task<ActionResult<NalogForm>> GetNalogForm(Guid id)
		{
			return Ok(await nalogFormRepo.GetSaStavkamaAsync(id));
		}

		[HttpGet("kartica-konta/{id}")]
		public async Task<ActionResult<IList<KarticaKonta>>> GetKarticaKonta(long id)
		{
			return Ok(await karticaKontaRepo.GetAllAsync(id));
		}
	}
}
