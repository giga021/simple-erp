using MassTransit;
using Moq;
using Pregledi.Application.NotificationHandlers;
using Pregledi.Application.Notifications;
using System.Threading.Tasks;
using Xunit;

namespace Pregledi.UnitTest.Application.NotificationHandlers
{
	public class GlavnaKnjigaChangedHandlerTest
	{
		[Fact]
		public async Task Handle_Korektno()
		{
			var notification = new GlavnaKnjigaChanged("123");
			var fakeBus = new Mock<IBus>();
			var handler = new GlavnaKnjigaChangedHandler(fakeBus.Object);

			await handler.Handle(notification, default);

			fakeBus.Verify(x => x.Publish(notification, default));
		}
	}
}
