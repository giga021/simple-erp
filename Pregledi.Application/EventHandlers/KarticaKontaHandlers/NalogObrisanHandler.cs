using MediatR;
using Microsoft.Extensions.Logging;
using Pregledi.Application.Events;
using Pregledi.Application.Notifications;
using Pregledi.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Pregledi.Application.EventHandlers.KarticaKontaHandlers
{
	public class NalogObrisanHandler : INotificationHandler<NalogObrisan>
	{
		private readonly ILogger<NalogObrisanHandler> logger;
		private readonly IKarticaKontaRepository karticaRepo;
		private readonly INotificationQueue notifications;

		public NalogObrisanHandler(IKarticaKontaRepository karticaRepo, INotificationQueue notifications,
			ILogger<NalogObrisanHandler> logger)
		{
			this.karticaRepo = karticaRepo;
			this.logger = logger;
			this.notifications = notifications;
		}

		public async Task Handle(NalogObrisan evnt, CancellationToken cancellationToken)
		{
			logger.LogTrace($"Handling IdNaloga: {evnt.IdNaloga}");

			var stavke = await karticaRepo.GetStavkeNalogaAsync(evnt.IdNaloga);

			foreach (var item in stavke)
			{
				karticaRepo.Remove(item);
			}
			notifications.Add(new KarticaKontaChanged());
		}
	}
}
