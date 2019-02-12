using MediatR;
using Microsoft.Extensions.Logging;
using Pregledi.Application.Events;
using Pregledi.Application.Exceptions;
using Pregledi.Domain.Entities;
using Pregledi.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Pregledi.Application.EventHandlers.StavkaFormHandlers
{
	public class StavkaProknjizenaHandler : INotificationHandler<StavkaProknjizena>
	{
		private readonly ILogger<StavkaProknjizenaHandler> logger;
		private readonly IStavkaFormRepository stavkaRepo;
		private readonly IKontoRepository kontoRepo;

		public StavkaProknjizenaHandler(IStavkaFormRepository stavkaRepo,
			IKontoRepository kontoRepo,
			ILogger<StavkaProknjizenaHandler> logger)
		{
			this.stavkaRepo = stavkaRepo;
			this.kontoRepo = kontoRepo;
			this.logger = logger;
		}

		public async Task Handle(StavkaProknjizena evnt, CancellationToken cancellationToken)
		{
			logger.LogTrace($"Handling IdStavke: {evnt.IdStavke}");
			var konto = await kontoRepo.GetAsync(evnt.IdKonto);

			if (konto == null)
				throw new PreglediException($"Konto {evnt.IdKonto} ne postoji");

			StavkaForm nalog = new StavkaForm()
			{
				Id = evnt.IdStavke,
				IdKonto = evnt.IdKonto,
				IdNaloga = evnt.IdNaloga,
				DatumKnjizenja = evnt.DatumKnjizenja,
				Konto = konto.Sifra,
				Opis = evnt.Opis,
				Duguje = evnt.Duguje,
				Potrazuje = evnt.Potrazuje
			};
			stavkaRepo.Add(nalog);
		}
	}
}
