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
	public class NalogOtkljucanHandler : INotificationHandler<NalogOtkljucan>
	{
		private readonly ILogger<NalogOtkljucanHandler> logger;
		private readonly INalogGKRepository nalogRepo;
		private readonly INotificationQueue notifications;

		public NalogOtkljucanHandler(INalogGKRepository nalogRepo,
			INotificationQueue notifications,
			ILogger<NalogOtkljucanHandler> logger)
		{
			this.nalogRepo = nalogRepo;
			this.logger = logger;
			this.notifications = notifications;
		}

		public async Task Handle(NalogOtkljucan evnt, CancellationToken cancellationToken)
		{
			logger.LogTrace($"Handling IdNaloga: {evnt.IdNaloga}");
			var nalog = await nalogRepo.GetAsync(evnt.IdNaloga);

			if (nalog == null)
				throw new NalogNePostojiException(evnt.IdNaloga);

			nalog.Zakljucan = false;
			notifications.Add(new GlavnaKnjigaChanged(evnt.UserId));
		}
	}
}
