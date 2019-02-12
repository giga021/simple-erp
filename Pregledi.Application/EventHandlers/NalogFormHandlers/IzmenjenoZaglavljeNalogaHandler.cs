using MediatR;
using Microsoft.Extensions.Logging;
using Pregledi.Application.Events;
using Pregledi.Application.Exceptions;
using Pregledi.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Pregledi.Application.EventHandlers.NalogFormHandlers
{
	public class IzmenjenoZaglavljeNalogaHandler : INotificationHandler<IzmenjenoZaglavljeNaloga>
	{
		private readonly ILogger<IzmenjenoZaglavljeNalogaHandler> logger;
		private readonly INalogFormRepository nalogRepo;

		public IzmenjenoZaglavljeNalogaHandler(INalogFormRepository nalogRepo,
			ILogger<IzmenjenoZaglavljeNalogaHandler> logger)
		{
			this.nalogRepo = nalogRepo;
			this.logger = logger;
		}

		public async Task Handle(IzmenjenoZaglavljeNaloga evnt, CancellationToken cancellationToken)
		{
			logger.LogTrace($"Handling IdNaloga: {evnt.IdNaloga}");
			var nalog = await nalogRepo.GetAsync(evnt.IdNaloga);

			if (nalog == null)
				throw new NalogNePostojiException(evnt.IdNaloga);

			nalog.Datum = evnt.DatumNaloga;
			nalog.IdTip = evnt.IdTip;
			nalog.Opis = evnt.Opis;
		}
	}
}
