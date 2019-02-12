using MediatR;
using Microsoft.Extensions.Logging;
using Pregledi.Application.Events;
using Pregledi.Application.Notifications;
using Pregledi.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Pregledi.Application.EventHandlers.KarticaKontaHandlers
{
	public class IzmenjenoZaglavljeNalogaHandler : INotificationHandler<IzmenjenoZaglavljeNaloga>
	{
		private readonly ILogger<IzmenjenoZaglavljeNalogaHandler> logger;
		private readonly IKarticaKontaRepository karticaRepo;
		private readonly ITipNalogaRepository tipRepo;
		private readonly INotificationQueue notifications;

		public IzmenjenoZaglavljeNalogaHandler(IKarticaKontaRepository karticaRepo, ITipNalogaRepository tipRepo,
			INotificationQueue notifications, ILogger<IzmenjenoZaglavljeNalogaHandler> logger)
		{
			this.karticaRepo = karticaRepo;
			this.tipRepo = tipRepo;
			this.logger = logger;
			this.notifications = notifications;
		}

		public async Task Handle(IzmenjenoZaglavljeNaloga evnt, CancellationToken cancellationToken)
		{
			logger.LogTrace($"Handling IdNaloga: {evnt.IdNaloga}");

			var stavkeNaloga = await karticaRepo.GetStavkeNalogaAsync(evnt.IdNaloga);
			var tip = await tipRepo.GetAsync(evnt.IdTip);

			foreach (var item in stavkeNaloga)
			{
				item.TipNaloga = tip.Naziv;
				item.DatumNaloga = evnt.DatumNaloga;
			}
			notifications.Add(new KarticaKontaChanged());
		}
	}
}
