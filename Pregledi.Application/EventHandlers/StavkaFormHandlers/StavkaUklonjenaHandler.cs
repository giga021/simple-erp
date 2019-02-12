using MediatR;
using Microsoft.Extensions.Logging;
using Pregledi.Application.Events;
using Pregledi.Application.Exceptions;
using Pregledi.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Pregledi.Application.EventHandlers.StavkaFormHandlers
{
	public class StavkaUklonjenaHandler : INotificationHandler<StavkaUklonjena>
	{
		private readonly ILogger<StavkaUklonjenaHandler> logger;
		private readonly IStavkaFormRepository stavkaRepo;

		public StavkaUklonjenaHandler(IStavkaFormRepository stavkaRepo,
			ILogger<StavkaUklonjenaHandler> logger)
		{
			this.stavkaRepo = stavkaRepo;
			this.logger = logger;
		}

		public async Task Handle(StavkaUklonjena evnt, CancellationToken cancellationToken)
		{
			logger.LogTrace($"Handling IdStavke: {evnt.IdStavke}");
			var stavka = await stavkaRepo.GetAsync(evnt.IdStavke);
			if (stavka == null)
				throw new PreglediException($"Stavka {evnt.IdStavke} ne postoji");

			stavkaRepo.Remove(stavka);
		}
	}
}
