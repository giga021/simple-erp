using Microsoft.Extensions.Logging;
using Moq;
using Pregledi.Application;
using Pregledi.Application.EventHandlers.GlavnaKnjigaHandlers;
using Pregledi.Application.Events;
using Pregledi.Application.Notifications;
using Pregledi.Domain.Entities;
using Pregledi.Domain.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Pregledi.UnitTest.Application.EventHandlers.GlavnaKnjigaHandlers
{
	public class NalogOtvorenHandlerTest
	{
		[Fact]
		public async Task Handle_Korektno()
		{
			var fakeRepo = new Mock<INalogGKRepository>();
			var fakeTipRepo = new Mock<ITipNalogaRepository>();
			fakeTipRepo.Setup(x => x.GetAsync(2)).ReturnsAsync(new TipNaloga { Naziv = "Izvodi" });
			var fakeNotifications = new Mock<INotificationQueue>();
			var fakeLogger = new Mock<ILogger<NalogOtvorenHandler>>();
			var evnt = new NalogOtvoren(Guid.NewGuid(), new DateTime(2018, 10, 21), 2, "opis novi")
			{
				UserId = Guid.NewGuid().ToString()
			};
			var handler = new NalogOtvorenHandler(fakeRepo.Object, fakeTipRepo.Object, fakeNotifications.Object, fakeLogger.Object);

			await handler.Handle(evnt, default);

			fakeRepo.Verify(x => x.Add(It.Is<NalogGlavnaKnjiga>(n => n.Datum == new DateTime(2018, 10, 21) &&
				  n.TipNaziv == "Izvodi" && n.Opis == "opis novi")));
			fakeNotifications.Verify(x => x.Add(It.Is<GlavnaKnjigaChanged>(n => n.UserId == evnt.UserId)));
		}
	}
}
