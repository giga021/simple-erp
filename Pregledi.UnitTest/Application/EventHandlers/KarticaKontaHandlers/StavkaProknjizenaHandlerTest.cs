using Microsoft.Extensions.Logging;
using Moq;
using Pregledi.Application;
using Pregledi.Application.EventHandlers.KarticaKontaHandlers;
using Pregledi.Application.Events;
using Pregledi.Application.Exceptions;
using Pregledi.Application.Notifications;
using Pregledi.Domain.Entities;
using Pregledi.Domain.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Pregledi.UnitTest.Application.EventHandlers.KarticaKontaHandlers
{
	public class StavkaProknjizenaHandlerTest
	{
		[Fact]
		public async Task KontoNePostoji_Greska()
		{
			var evnt = new StavkaProknjizena(Guid.NewGuid(), Guid.NewGuid(), new DateTime(2018, 10, 22), 1, 100, 0, "opis stavke");
			var fakeRepo = new Mock<IKarticaKontaRepository>();
			var fakeGkRepo = new Mock<INalogGKRepository>();
			fakeGkRepo.Setup(x => x.GetAsync(evnt.IdNaloga)).ReturnsAsync(new NalogGlavnaKnjiga());
			var fakeKontoRepo = new Mock<IKontoRepository>();
			var fakeNotifications = new Mock<INotificationQueue>();
			var fakeLogger = new Mock<ILogger<StavkaProknjizenaHandler>>();
			var handler = new StavkaProknjizenaHandler(fakeRepo.Object, fakeKontoRepo.Object, fakeGkRepo.Object,
				fakeNotifications.Object, fakeLogger.Object);

			Func<Task> handle = async () => await handler.Handle(evnt, default);

			await Assert.ThrowsAsync<PreglediException>(handle);
		}

		[Fact]
		public async Task NalogNePostoji_Greska()
		{
			var evnt = new StavkaProknjizena(Guid.NewGuid(), Guid.NewGuid(), new DateTime(2018, 10, 22), 1, 100, 0, "opis stavke");
			var fakeRepo = new Mock<IKarticaKontaRepository>();
			var fakeGkRepo = new Mock<INalogGKRepository>();
			var fakeKontoRepo = new Mock<IKontoRepository>();
			fakeKontoRepo.Setup(x => x.GetAsync(evnt.IdKonto)).ReturnsAsync(new Konto());
			var fakeNotifications = new Mock<INotificationQueue>();
			var fakeLogger = new Mock<ILogger<StavkaProknjizenaHandler>>();
			var handler = new StavkaProknjizenaHandler(fakeRepo.Object, fakeKontoRepo.Object, fakeGkRepo.Object,
				fakeNotifications.Object, fakeLogger.Object);

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
			var evnt = new StavkaProknjizena(Guid.NewGuid(), Guid.NewGuid(), new DateTime(2018, 10, 22), 1, duguje, potrazuje, "opis stavke");
			var nalog = new NalogGlavnaKnjiga() { Id = evnt.IdNaloga, Datum = new DateTime(2018, 10, 22) };
			var fakeRepo = new Mock<IKarticaKontaRepository>();
			var fakeGkRepo = new Mock<INalogGKRepository>();
			fakeGkRepo.Setup(x => x.GetAsync(evnt.IdNaloga)).ReturnsAsync(nalog);
			var fakeKontoRepo = new Mock<IKontoRepository>();
			var konto = new Konto() { Id = evnt.IdKonto, Sifra = "410" };
			fakeKontoRepo.Setup(x => x.GetAsync(evnt.IdKonto)).ReturnsAsync(konto);
			var fakeNotifications = new Mock<INotificationQueue>();
			var fakeLogger = new Mock<ILogger<StavkaProknjizenaHandler>>();
			var handler = new StavkaProknjizenaHandler(fakeRepo.Object, fakeKontoRepo.Object, fakeGkRepo.Object,
				fakeNotifications.Object, fakeLogger.Object);

			await handler.Handle(evnt, default);

			fakeRepo.Verify(x => x.Add(It.Is<KarticaKonta>(s => s.Id == evnt.IdStavke && s.IdNaloga == evnt.IdNaloga &&
				s.IdKonto == evnt.IdKonto && s.Saldo == (evnt.Duguje - evnt.Potrazuje) && s.Duguje == evnt.Duguje &&
				s.Potrazuje == evnt.Potrazuje && s.DatumNaloga == nalog.Datum && s.DatumKnjizenja == evnt.DatumKnjizenja &&
				s.Created == evnt.Created && s.Konto == konto.Sifra)));
			fakeNotifications.Verify(x => x.Add(It.IsNotNull<KarticaKontaChanged>()));
		}
	}
}
