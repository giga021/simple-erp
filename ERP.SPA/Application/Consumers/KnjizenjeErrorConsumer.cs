using ERP.SPA.Hubs;
using Integration.Contracts.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ERP.SPA.Application.Consumers
{
	public class KnjizenjeErrorConsumer : IConsumer<IKnjizenjeError>
	{
		private readonly IHubContext<GlavnaKnjigaHub> gkHub;

		public KnjizenjeErrorConsumer(IHubContext<GlavnaKnjigaHub> gkHub)
		{
			this.gkHub = gkHub;
		}

		public async Task Consume(ConsumeContext<IKnjizenjeError> context)
		{
			await gkHub.Clients.All.SendAsync("knjizenje-error", context.Message);
		}
	}
}
