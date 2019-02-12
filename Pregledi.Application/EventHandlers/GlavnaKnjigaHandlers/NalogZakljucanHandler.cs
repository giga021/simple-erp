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
	public class NalogZakljucanHandler : INotificationHandler<NalogZakljucan>
	{
		private readonly ILogger<NalogZakljucanHandler> logger;
		private readonly INalogGKRepository nalogRepo;
		private readonly INotificationQueue notifications;

		public NalogZakljucanHandler(INalogGKRepository nalogRepo, INotificationQueue notifications,
			ILogger<NalogZakljucanHandler> logger)
		{
			this.nalogRepo = nalogRepo;
			this.logger = logger;
			this.notifications = notifications;
		}

		public async Task Handle(NalogZakljucan evnt, CancellationToken cancellationToken)
		{
			logger.LogTrace($"Handling IdNaloga: {evnt.IdNaloga}");
			var nalog = await nalogRepo.GetAsync(evnt.IdNaloga);

			if (nalog == null)
				throw new NalogNePostojiException(evnt.IdNaloga);

			nalog.Zakljucan = true;
			notifications.Add(new GlavnaKnjigaChanged(evnt.UserId));
		}
	}
}
