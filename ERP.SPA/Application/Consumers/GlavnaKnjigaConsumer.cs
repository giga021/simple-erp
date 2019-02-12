using ERP.SPA.Hubs;
using Integration.Contracts.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ERP.SPA.Application.Consumers
{
	public class GlavnaKnjigaConsumer : IConsumer<IGlavnaKnjigaChanged>
	{
		private readonly IHubContext<GlavnaKnjigaHub> gkHub;

		public GlavnaKnjigaConsumer(IHubContext<GlavnaKnjigaHub> gkHub)
		{
			this.gkHub = gkHub;
		}

		public async Task Consume(ConsumeContext<IGlavnaKnjigaChanged> context)
		{
			await gkHub.Clients.User(context.Message.UserId).SendAsync("data-changed");
		}
	}
}
