using MediatR;
using Microsoft.Extensions.Logging;
using Pregledi.Application.Events;
using Pregledi.Application.Exceptions;
using Pregledi.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Pregledi.Application.EventHandlers.NalogFormHandlers
{
	public class NalogObrisanHandler : INotificationHandler<NalogObrisan>
	{
		private readonly ILogger<NalogObrisanHandler> logger;
		private readonly INalogFormRepository nalogRepo;

		public NalogObrisanHandler(INalogFormRepository nalogRepo,
			ILogger<NalogObrisanHandler> logger)
		{
			this.nalogRepo = nalogRepo;
			this.logger = logger;
		}

		public async Task Handle(NalogObrisan evnt, CancellationToken cancellationToken)
		{
			logger.LogTrace($"Handling IdNaloga: {evnt.IdNaloga}");
			var nalog = await nalogRepo.GetAsync(evnt.IdNaloga);

			if (nalog == null)
				throw new NalogNePostojiException(evnt.IdNaloga);

			nalogRepo.Remove(nalog);
		}
	}
}
