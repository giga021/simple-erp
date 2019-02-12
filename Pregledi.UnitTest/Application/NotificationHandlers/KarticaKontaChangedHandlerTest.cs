using Moq;
using Pregledi.Application.NotificationHandlers;
using Pregledi.Application.Notifications;
using Pregledi.Domain.Repositories;
using System.Threading.Tasks;
using Xunit;

namespace Pregledi.UnitTest.Application.NotificationHandlers
{
	public class KarticaKontaChangedHandlerTest
	{
		[Fact]
		public async Task Handle_Korektno()
		{
			var notification = new KarticaKontaChanged();
			var fakeRepo = new Mock<IKarticaKontaRepository>();
			var handler = new KarticaKontaChangedHandler(fakeRepo.Object);

			await handler.Handle(notification, default);

			fakeRepo.Verify(x => x.IzracunajAsync());
		}
	}
}
