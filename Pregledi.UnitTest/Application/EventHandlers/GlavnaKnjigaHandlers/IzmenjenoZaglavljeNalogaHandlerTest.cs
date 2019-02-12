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
	public class IzmenjenoZaglavljeNalogaHandlerTest
	{
		[Fact]
		public async Task NalogNePostoji_Greska()
		{
			var fakeRepo = new Mock<INalogGKRepository>();
			var fakeTipRepo = new Mock<ITipNalogaRepository>();
			var fakeNotifications = new Mock<INotificationQueue>();
			var fakeLogger = new Mock<ILogger<IzmenjenoZaglavljeNalogaHandler>>();
			var evnt = new IzmenjenoZaglavljeNaloga(Guid.NewGuid(), new DateTime(2018, 10, 21), 1, "opis");
			var handler = new IzmenjenoZaglavljeNalogaHandler(fakeRepo.Object, fakeTipRepo.Object, fakeNotifications.Object, fakeLogger.Object);

			Func<Task> handle = async () => await handler.Handle(evnt, default);

			await Assert.ThrowsAsync<NalogNePostojiException>(handle);
		}

		[Fact]
		public async Task Handle_Korektno()
		{
			var nalogIzBaze = new NalogGlavnaKnjiga()
			{
				Id = Guid.NewGuid(),
				Datum = new DateTime(2018, 10, 20),
				TipNaziv = "Ulazne fakture",
				Opis = "opis"
			};
			var fakeRepo = new Mock<INalogGKRepository>();
			fakeRepo.Setup(x => x.GetAsync(nalogIzBaze.Id)).ReturnsAsync(nalogIzBaze);
			var fakeTipRepo = new Mock<ITipNalogaRepository>();
			fakeTipRepo.Setup(x => x.GetAsync(2)).ReturnsAsync(new TipNaloga { Naziv = "Izvodi" });
			var fakeNotifications = new Mock<INotificationQueue>();
			var fakeLogger = new Mock<ILogger<IzmenjenoZaglavljeNalogaHandler>>();
			var evnt = new IzmenjenoZaglavljeNaloga(nalogIzBaze.Id, new DateTime(2018, 10, 21), 2, "opis novi")
			{
				UserId = Guid.NewGuid().ToString()
			};
			var handler = new IzmenjenoZaglavljeNalogaHandler(fakeRepo.Object, fakeTipRepo.Object, fakeNotifications.Object, fakeLogger.Object);

			await handler.Handle(evnt, default);

			Assert.Equal(new DateTime(2018, 10, 21), nalogIzBaze.Datum);
			Assert.Equal("Izvodi", nalogIzBaze.TipNaziv);
			Assert.Equal("opis novi", nalogIzBaze.Opis);
			fakeNotifications.Verify(x => x.Add(It.Is<GlavnaKnjigaChanged>(n => n.UserId == evnt.UserId)));
		}
	}
}
