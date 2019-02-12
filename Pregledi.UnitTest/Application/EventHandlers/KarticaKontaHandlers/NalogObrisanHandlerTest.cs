using Microsoft.Extensions.Logging;
using Moq;
using Pregledi.Application;
using Pregledi.Application.EventHandlers.KarticaKontaHandlers;
using Pregledi.Application.Events;
using Pregledi.Application.Notifications;
using Pregledi.Domain.Entities;
using Pregledi.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Pregledi.UnitTest.Application.EventHandlers.KarticaKontaHandlers
{
	public class NalogObrisanHandlerTest
	{
		[Fact]
		public async Task Handle_Korektno()
		{
			var stavkeNaloga = new List<KarticaKonta>()
			{
				new KarticaKonta() { IdKonto = 1, Saldo = 100, SaldoKumulativno = 100 },
				new KarticaKonta() { IdKonto = 2, Saldo = 500, SaldoKumulativno = 100 },
				new KarticaKonta() { IdKonto = 1, Saldo = 200, SaldoKumulativno = 400 },
				new KarticaKonta() { IdKonto = 2, Saldo = 300, SaldoKumulativno = 375 },
			};
			var evnt = new NalogObrisan(Guid.NewGuid());
			var fakeRepo = new Mock<IKarticaKontaRepository>();
			var fakeNotifications = new Mock<INotificationQueue>();
			fakeRepo.Setup(x => x.GetStavkeNalogaAsync(evnt.IdNaloga)).ReturnsAsync(stavkeNaloga);
			var fakeLogger = new Mock<ILogger<NalogObrisanHandler>>();
			var handler = new NalogObrisanHandler(fakeRepo.Object, fakeNotifications.Object, fakeLogger.Object);

			await handler.Handle(evnt, default);

			foreach (var item in stavkeNaloga)
			{
				fakeRepo.Verify(x => x.Remove(item));
			}
			fakeNotifications.Verify(x => x.Add(It.IsNotNull<KarticaKontaChanged>()));
		}
	}
}
