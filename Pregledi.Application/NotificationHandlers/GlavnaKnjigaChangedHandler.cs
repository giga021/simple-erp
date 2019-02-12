using MassTransit;
using MediatR;
using Pregledi.Application.Notifications;
using System.Threading;
using System.Threading.Tasks;

namespace Pregledi.Application.NotificationHandlers
{
	public class GlavnaKnjigaChangedHandler : INotificationHandler<GlavnaKnjigaChanged>
	{
		private readonly IBus eventBus;

		public GlavnaKnjigaChangedHandler(IBus eventBus)
		{
			this.eventBus = eventBus;
		}

		public async Task Handle(GlavnaKnjigaChanged notification, CancellationToken cancellationToken)
		{
			await eventBus.Publish(notification);
		}
	}
}
