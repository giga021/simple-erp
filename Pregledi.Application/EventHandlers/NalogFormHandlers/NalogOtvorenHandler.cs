using MediatR;
using Microsoft.Extensions.Logging;
using Pregledi.Application.Events;
using Pregledi.Domain.Entities;
using Pregledi.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Pregledi.Application.EventHandlers.NalogFormHandlers
{
	public class NalogOtvorenHandler : INotificationHandler<NalogOtvoren>
	{
		private readonly ILogger<NalogOtvorenHandler> logger;
		private readonly INalogFormRepository nalogRepo;

		public NalogOtvorenHandler(INalogFormRepository nalogRepo,
			ILogger<NalogOtvorenHandler> logger)
		{
			this.nalogRepo = nalogRepo;
			this.logger = logger;
		}

		public Task Handle(NalogOtvoren evnt, CancellationToken cancellationToken)
		{
			logger.LogTrace($"Handling IdNaloga: {evnt.IdNaloga}");
			NalogForm nalog = new NalogForm()
			{
				Id = evnt.IdNaloga,
				IdTip = evnt.IdTip,
				Datum = evnt.DatumNaloga,
				Opis = evnt.Opis,
			};
			nalogRepo.Add(nalog);
			return Task.CompletedTask;
		}
	}
}
