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
	public class StavkaProknjizenaHandler : INotificationHandler<StavkaProknjizena>
	{
		private readonly ILogger<StavkaProknjizenaHandler> logger;
		private readonly INalogGKRepository nalogRepo;
		private readonly INotificationQueue notifications;

		public StavkaProknjizenaHandler(INalogGKRepository nalogRepo,
			INotificationQueue notifications,
			ILogger<StavkaProknjizenaHandler> logger)
		{
			this.nalogRepo = nalogRepo;
			this.logger = logger;
			this.notifications = notifications;
		}

		public async Task Handle(StavkaProknjizena evnt, CancellationToken cancellationToken)
		{
			logger.LogTrace($"Handling IdNaloga: {evnt.IdNaloga}");
			var nalog = await nalogRepo.GetAsync(evnt.IdNaloga);

			if (nalog == null)
				throw new NalogNePostojiException(evnt.IdNaloga);

			nalog.BrojStavki++;
			nalog.UkupnoDuguje += evnt.Duguje;
			nalog.UkupnoPotrazuje += evnt.Potrazuje;
			nalog.UkupnoSaldo = nalog.UkupnoDuguje - nalog.UkupnoPotrazuje;
			notifications.Add(new GlavnaKnjigaChanged(evnt.UserId));
		}
	}
}
