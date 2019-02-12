using ERP.SPA.Application.Commands;
using ERP.SPA.DTO;
using ERP.SPA.Infrastructure;
using ERP.SPA.Infrastructure.Endpoints;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.SPA.Application.Services
{
	public interface IKnjizenjeService
	{
		Task SnimiNoviAsync(NalogFormDTO nalog);
		Task SnimiPromenuAsync(NalogFormDTO nalog);
		Task ZakljucajAsync(Guid idNaloga, long version);
		Task OtkljucajAsync(Guid idNaloga, long version);
		Task ObrisiAsync(Guid idNaloga, long version);
	}

	public class KnjizenjeService : IKnjizenjeService
	{
		private readonly IKnjizenjeSendEndpoint queue;
		private readonly IIdentityService identitySvc;

		public KnjizenjeService(IKnjizenjeSendEndpoint queue, IIdentityService identitySvc)
		{
			this.queue = queue;
			this.identitySvc = identitySvc;
		}

		public async Task SnimiNoviAsync(NalogFormDTO nalog)
		{
			var stavke = nalog.Stavke
				.Select(s => new StavkaDTO(null, s.IdKonto, s.Duguje, s.Potrazuje, s.Opis, s.Stornirana));
			var command = new OtvoriNalog(Guid.NewGuid(), identitySvc.GetUserId(), nalog.IdTip, nalog.Datum,
				nalog.Opis, stavke);
			await queue.Endpoint.Send(command);
		}

		public async Task SnimiPromenuAsync(NalogFormDTO nalog)
		{
			var stavke = nalog.Stavke
				.Select(s => new StavkaDTO(s.Id, s.IdKonto, s.Duguje, s.Potrazuje, s.Opis, s.Stornirana));
			var command = new IzmeniNalog(Guid.NewGuid(), nalog.Version.Value,
				identitySvc.GetUserId(), nalog.Id.Value, nalog.IdTip, nalog.Datum, nalog.Opis, stavke);
			await queue.Endpoint.Send(command);
		}

		public async Task ZakljucajAsync(Guid idNaloga, long version)
		{
			var command = new ZakljucajNalog(Guid.NewGuid(), version, identitySvc.GetUserId(), idNaloga);
			await queue.Endpoint.Send(command);
		}

		public async Task OtkljucajAsync(Guid idNaloga, long version)
		{
			var command = new OtkljucajNalog(Guid.NewGuid(), version, identitySvc.GetUserId(), idNaloga);
			await queue.Endpoint.Send(command);
		}

		public async Task ObrisiAsync(Guid idNaloga, long version)
		{
			var command = new ObrisiNalog(Guid.NewGuid(), version, identitySvc.GetUserId(), idNaloga);
			await queue.Endpoint.Send(command);
		}
	}
}
