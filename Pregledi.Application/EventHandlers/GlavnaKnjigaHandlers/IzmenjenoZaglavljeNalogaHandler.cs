using MediatR;
using Microsoft.Extensions.Logging;
using Pregledi.Application.Events;
using Pregledi.Application.Exceptions;
using Pregledi.Application.Notifications;
using Pregledi.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Pregledi.Application.EventHandlers.GlavnaKnjigaHandlers
{
	public class IzmenjenoZaglavljeNalogaHandler : INotificationHandler<IzmenjenoZaglavljeNaloga>
	{
		private readonly ILogger<IzmenjenoZaglavljeNalogaHandler> logger;
		private readonly INalogGKRepository nalogRepo;
		private readonly ITipNalogaRepository tipNalogaRepo;
		private readonly INotificationQueue notifications;

		public IzmenjenoZaglavljeNalogaHandler(INalogGKRepository nalogRepo,
			ITipNalogaRepository tipNalogaRepo, INotificationQueue notifications,
			ILogger<IzmenjenoZaglavljeNalogaHandler> logger)
		{
			this.nalogRepo = nalogRepo;
			this.tipNalogaRepo = tipNalogaRepo;
			this.logger = logger;
			this.notifications = notifications;
		}

		public async Task Handle(IzmenjenoZaglavljeNaloga evnt, CancellationToken cancellationToken)
		{
			logger.LogTrace($"Handling IdNaloga: {evnt.IdNaloga}");
			var nalog = await nalogRepo.GetAsync(evnt.IdNaloga);

			if (nalog == null)
				throw new NalogNePostojiException(evnt.IdNaloga);

			var tip = await tipNalogaRepo.GetAsync(evnt.IdTip);
			nalog.Datum = evnt.DatumNaloga;
			nalog.TipNaziv = tip.Naziv;
			nalog.Opis = evnt.Opis;
			notifications.Add(new GlavnaKnjigaChanged(evnt.UserId));
		}
	}
}
