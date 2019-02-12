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
	public class NalogZakljucanHandlerTest
	{
		[Fact]
		public async Task NalogNePostoji_Greska()
		{
			var fakeRepo = new Mock<INalogGKRepository>();
			var fakeNotifications = new Mock<INotificationQueue>();
			var fakeLogger = new Mock<ILogger<NalogZakljucanHandler>>();
			var evnt = new NalogZakljucan(Guid.NewGuid());
			var handler = new NalogZakljucanHandler(fakeRepo.Object, fakeNotifications.Object, fakeLogger.Object);

			Func<Task> handle = async () => await handler.Handle(evnt, default);

			await Assert.ThrowsAsync<NalogNePostojiException>(handle);
		}

		[Fact]
		public async Task Handle_Korektno()
		{
			var nalogIzBaze = new NalogGlavnaKnjiga()
			{
				Id = Guid.NewGuid(),
				Zakljucan = false
			};
			var fakeRepo = new Mock<INalogGKRepository>();
			fakeRepo.Setup(x => x.GetAsync(nalogIzBaze.Id)).ReturnsAsync(nalogIzBaze);
			var fakeNotifications = new Mock<INotificationQueue>();
			var fakeLogger = new Mock<ILogger<NalogZakljucanHandler>>();
			var evnt = new NalogZakljucan(nalogIzBaze.Id)
			{
				UserId = Guid.NewGuid().ToString()
			};
			var handler = new NalogZakljucanHandler(fakeRepo.Object, fakeNotifications.Object, fakeLogger.Object);

			await handler.Handle(evnt, default);

			Assert.True(nalogIzBaze.Zakljucan);
			fakeNotifications.Verify(x => x.Add(It.Is<GlavnaKnjigaChanged>(n => n.UserId == evnt.UserId)));
		}
	}
}
