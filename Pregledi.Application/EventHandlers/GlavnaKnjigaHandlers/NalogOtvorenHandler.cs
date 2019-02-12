using MediatR;
using Microsoft.Extensions.Logging;
using Pregledi.Application.Events;
using Pregledi.Application.Notifications;
using Pregledi.Domain.Entities;
using Pregledi.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Pregledi.Application.EventHandlers.GlavnaKnjigaHandlers
{
	public class NalogOtvorenHandler : INotificationHandler<NalogOtvoren>
	{
		private readonly ILogger<NalogOtvorenHandler> logger;
		private readonly INalogGKRepository nalogRepo;
		private readonly ITipNalogaRepository tipNalogaRepo;
		private readonly INotificationQueue notifications;

		public NalogOtvorenHandler(INalogGKRepository nalogRepo,
			ITipNalogaRepository tipNalogaRepo, INotificationQueue notifications,
			ILogger<NalogOtvorenHandler> logger)
		{
			this.nalogRepo = nalogRepo;
			this.tipNalogaRepo = tipNalogaRepo;
			this.logger = logger;
			this.notifications = notifications;
		}

		public async Task Handle(NalogOtvoren evnt, CancellationToken cancellationToken)
		{
			logger.LogTrace($"Handling IdNaloga: {evnt.IdNaloga}");
			var tip = await tipNalogaRepo.GetAsync(evnt.IdTip);
			NalogGlavnaKnjiga nalog = new NalogGlavnaKnjiga()
			{
				Id = evnt.IdNaloga,
				Datum = evnt.DatumNaloga,
				TipNaziv = tip.Naziv,
				Opis = evnt.Opis,
				BrojStavki = 0,
				UkupnoDuguje = 0,
				UkupnoPotrazuje = 0,
				UkupnoSaldo = 0,
				Zakljucan = false
			};
			nalogRepo.Add(nalog);
			notifications.Add(new GlavnaKnjigaChanged(evnt.UserId));
		}
	}
}
