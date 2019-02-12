using MediatR;
using Pregledi.Application.Notifications;
using Pregledi.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Pregledi.Application.NotificationHandlers
{
	public class KarticaKontaChangedHandler : INotificationHandler<KarticaKontaChanged>
	{
		private readonly IKarticaKontaRepository karticaRepo;

		public KarticaKontaChangedHandler(IKarticaKontaRepository karticaRepo)
		{
			this.karticaRepo = karticaRepo;
		}

		public async Task Handle(KarticaKontaChanged notification, CancellationToken cancellationToken)
		{
			await karticaRepo.IzracunajAsync();
		}
	}
}
