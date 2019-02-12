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
	public class IzmenjenoZaglavljeNalogaHandlerTest
	{
		[Fact]
		public async Task Handle_MenjaTip_Korektno()
		{
			var stavkeNaloga = new List<KarticaKonta>()
			{
				new KarticaKonta() { TipNaloga = "Izvodi", DatumNaloga = new DateTime(2018, 10, 20) },
				new KarticaKonta() { TipNaloga = "Izvodi", DatumNaloga = new DateTime(2018, 10, 20) },
			};
			var evnt = new IzmenjenoZaglavljeNaloga(Guid.NewGuid(), new DateTime(2018, 10, 21), 2, "novi opis");
			var fakeRepo = new Mock<IKarticaKontaRepository>();
			fakeRepo.Setup(x => x.GetStavkeNalogaAsync(evnt.IdNaloga)).ReturnsAsync(stavkeNaloga);
			var fakeTipRepo = new Mock<ITipNalogaRepository>();
			fakeTipRepo.Setup(x => x.GetAsync(evnt.IdTip)).ReturnsAsync(new TipNaloga { Naziv = "Novi tip" });
			var fakeNotifications = new Mock<INotificationQueue>();
			var fakeLogger = new Mock<ILogger<IzmenjenoZaglavljeNalogaHandler>>();
			var handler = new IzmenjenoZaglavljeNalogaHandler(fakeRepo.Object, fakeTipRepo.Object, fakeNotifications.Object, fakeLogger.Object);

			await handler.Handle(evnt, default);

			Assert.All(stavkeNaloga, x =>
			{
				Assert.Equal("Novi tip", x.TipNaloga);
				Assert.Equal(evnt.DatumNaloga, x.DatumNaloga);
			});
			fakeNotifications.Verify(x => x.Add(It.IsNotNull<KarticaKontaChanged>()));
		}
	}
}
