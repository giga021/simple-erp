using MediatR;
using Microsoft.Extensions.Logging;
using Pregledi.Application.Events;
using Pregledi.Application.Exceptions;
using Pregledi.Application.Notifications;
using Pregledi.Domain.Entities;
using Pregledi.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Pregledi.Application.EventHandlers.KarticaKontaHandlers
{
	public class StavkaProknjizenaHandler : INotificationHandler<StavkaProknjizena>
	{
		private readonly ILogger<StavkaProknjizenaHandler> logger;
		private readonly IKarticaKontaRepository karticaRepo;
		private readonly INalogGKRepository nalogRepo;
		private readonly IKontoRepository kontoRepo;
		private readonly INotificationQueue notifications;

		public StavkaProknjizenaHandler(IKarticaKontaRepository karticaRepo,
			IKontoRepository kontoRepo, INalogGKRepository nalogRepo, INotificationQueue notifications,
			ILogger<StavkaProknjizenaHandler> logger)
		{
			this.karticaRepo = karticaRepo;
			this.kontoRepo = kontoRepo;
			this.nalogRepo = nalogRepo;
			this.logger = logger;
			this.notifications = notifications;
		}

		public async Task Handle(StavkaProknjizena evnt, CancellationToken cancellationToken)
		{
			logger.LogTrace($"Handling IdNaloga: {evnt.IdStavke}");

			var konto = await kontoRepo.GetAsync(evnt.IdKonto);
			if (konto == null)
				throw new PreglediException($"Konto {evnt.IdKonto} ne postoji");
			var nalog = await nalogRepo.GetAsync(evnt.IdNaloga);
			if (nalog == null)
				throw new NalogNePostojiException(evnt.IdNaloga);

			KarticaKonta stavka = new KarticaKonta()
			{
				Id = evnt.IdStavke,
				IdNaloga = evnt.IdNaloga,
				DatumKnjizenja = evnt.DatumKnjizenja,
				DatumNaloga = nalog.Datum,
				IdKonto = evnt.IdKonto,
				Konto = konto.Sifra,
				Opis = evnt.Opis,
				TipNaloga = nalog.TipNaziv,
				Duguje = evnt.Duguje,
				Potrazuje = evnt.Potrazuje,
				Saldo = evnt.Duguje - evnt.Potrazuje,
				Created = evnt.Created
			};

			karticaRepo.Add(stavka);
			notifications.Add(new KarticaKontaChanged());
		}
	}
}
