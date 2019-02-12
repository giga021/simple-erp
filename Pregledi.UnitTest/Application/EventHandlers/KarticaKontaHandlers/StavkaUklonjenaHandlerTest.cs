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
	public class StavkaUklonjenaHandlerTest
	{
		[Fact]
		public async Task StavkaNePostoji_Greska()
		{
			var fakeRepo = new Mock<IKarticaKontaRepository>();
			var fakeNotifications = new Mock<INotificationQueue>();
			var fakeLogger = new Mock<ILogger<StavkaUklonjenaHandler>>();
			var evnt = new StavkaUklonjena(Guid.NewGuid(), Guid.NewGuid(), new DateTime(2018, 10, 22), 1, 100, 0, "opis stavke");
			var handler = new StavkaUklonjenaHandler(fakeRepo.Object, fakeNotifications.Object, fakeLogger.Object);

			Func<Task> handle = async () => await handler.Handle(evnt, default);

			await Assert.ThrowsAsync<PreglediException>(handle);
		}

		[Fact]
		public async Task Handle_Korektno()
		{
			var stavkaIzBaze = new KarticaKonta()
			{
				Id = Guid.NewGuid(),
				IdNaloga = Guid.NewGuid(),
				IdKonto = 1,
				DatumNaloga = new DateTime(2018, 10, 20),
				DatumKnjizenja = new DateTime(2018, 10, 21),
				Created = new DateTime(2018, 10, 21),
				Duguje = 50,
				Potrazuje = 0,
				Saldo = 50,
				Opis = "opis"
			};
			var fakeRepo = new Mock<IKarticaKontaRepository>();
			fakeRepo.Setup(x => x.GetAsync(stavkaIzBaze.Id)).ReturnsAsync(stavkaIzBaze);
			var fakeNotifications = new Mock<INotificationQueue>();
			var fakeLogger = new Mock<ILogger<StavkaUklonjenaHandler>>();
			var evnt = new StavkaUklonjena(stavkaIzBaze.IdNaloga, stavkaIzBaze.Id, stavkaIzBaze.DatumKnjizenja,
				stavkaIzBaze.IdKonto, stavkaIzBaze.Duguje, stavkaIzBaze.Potrazuje, stavkaIzBaze.Opis);
			var handler = new StavkaUklonjenaHandler(fakeRepo.Object, fakeNotifications.Object, fakeLogger.Object);

			await handler.Handle(evnt, default);

			fakeRepo.Verify(x => x.Remove(stavkaIzBaze));
			fakeNotifications.Verify(x => x.Add(It.IsNotNull<KarticaKontaChanged>()));
		}
	}
}
