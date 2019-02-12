using MediatR;
using Microsoft.Extensions.Logging;
using Pregledi.Application.Events;
using Pregledi.Application.Exceptions;
using Pregledi.Application.Notifications;
using Pregledi.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Pregledi.Application.EventHandlers.KarticaKontaHandlers
{
	public class StavkaUklonjenaHandler : INotificationHandler<StavkaUklonjena>
	{
		private readonly ILogger<StavkaUklonjenaHandler> logger;
		private readonly IKarticaKontaRepository karticaRepo;
		private readonly INotificationQueue notifications;

		public StavkaUklonjenaHandler(IKarticaKontaRepository karticaRepo, INotificationQueue notifications,
			ILogger<StavkaUklonjenaHandler> logger)
		{
			this.karticaRepo = karticaRepo;
			this.logger = logger;
			this.notifications = notifications;
		}

		public async Task Handle(StavkaUklonjena evnt, CancellationToken cancellationToken)
		{
			logger.LogTrace($"Handling IdNaloga: {evnt.IdStavke}");

			var stavka = await karticaRepo.GetAsync(evnt.IdStavke);
			if (stavka == null)
				throw new PreglediException($"Stavka {evnt.IdStavke} ne postoji");

			karticaRepo.Remove(stavka);
			notifications.Add(new KarticaKontaChanged());
		}
	}
}
