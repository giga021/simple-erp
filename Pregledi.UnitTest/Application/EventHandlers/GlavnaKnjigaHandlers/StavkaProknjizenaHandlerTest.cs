using Microsoft.Extensions.Logging;
using Moq;
using Pregledi.Application;
using Pregledi.Application.Events;
using Pregledi.Application.Exceptions;
using Pregledi.Application.EventHandlers.GlavnaKnjigaHandlers;
using Pregledi.Application.Notifications;
using Pregledi.Domain.Entities;
using Pregledi.Domain.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Pregledi.UnitTest.Application.EventHandlers.GlavnaKnjigaHandlers
{
	public class StavkaProknjizenaHandlerTest
	{
		[Fact]
		public async Task NalogNePostoji_Greska()
		{
			var fakeRepo = new Mock<INalogGKRepository>();
			var fakeNotifications = new Mock<INotificationQueue>();
			var fakeLogger = new Mock<ILogger<StavkaProknjizenaHandler>>();
			var evnt = new StavkaProknjizena(Guid.NewGuid(), Guid.NewGuid(), new DateTime(2018, 10, 22), 1, 100, 0, "opis stavke");
			var handler = new StavkaProknjizenaHandler(fakeRepo.Object, fakeNotifications.Object, fakeLogger.Object);

			Func<Task> handle = async () => await handler.Handle(evnt, default);

			await Assert.ThrowsAsync<NalogNePostojiException>(handle);
		}

		[Theory]
		[InlineData(50, 0)]
		[InlineData(0, 50)]
		[InlineData(-50, 0)]
		[InlineData(0, -50)]
		public async Task Handle_Korektno(decimal duguje, decimal potrazuje)
		{
			int brojStavki = 7;
			decimal ukupnoDuguje = 400;
			decimal ukupnoPotrazuje = 700;
			decimal ukupnoSaldo = -300;
			var nalogIzBaze = new NalogGlavnaKnjiga()
			{
				Id = Guid.NewGuid(),
				BrojStavki = brojStavki,
				UkupnoDuguje = ukupnoDuguje,
				UkupnoPotrazuje = ukupnoPotrazuje,
				UkupnoSaldo = ukupnoSaldo
			};
			var fakeRepo = new Mock<INalogGKRepository>();
			fakeRepo.Setup(x => x.GetAsync(nalogIzBaze.Id)).ReturnsAsync(nalogIzBaze);
			var fakeNotifications = new Mock<INotificationQueue>();
			var fakeLogger = new Mock<ILogger<StavkaProknjizenaHandler>>();
			var evnt = new StavkaProknjizena(nalogIzBaze.Id, Guid.NewGuid(), new DateTime(2018, 10, 22), 1, duguje, potrazuje, "opis stavke")
			{
				UserId = Guid.NewGuid().ToString()
			};
			var handler = new StavkaProknjizenaHandler(fakeRepo.Object, fakeNotifications.Object, fakeLogger.Object);

			await handler.Handle(evnt, default);

			Assert.Equal(brojStavki + 1, nalogIzBaze.BrojStavki);
			Assert.Equal(ukupnoDuguje + duguje, nalogIzBaze.UkupnoDuguje);
			Assert.Equal(ukupnoPotrazuje + potrazuje, nalogIzBaze.UkupnoPotrazuje);
			Assert.Equal(nalogIzBaze.UkupnoDuguje - nalogIzBaze.UkupnoPotrazuje, nalogIzBaze.UkupnoSaldo);
			fakeNotifications.Verify(x => x.Add(It.Is<GlavnaKnjigaChanged>(n => n.UserId == evnt.UserId)));
		}
	}
}
