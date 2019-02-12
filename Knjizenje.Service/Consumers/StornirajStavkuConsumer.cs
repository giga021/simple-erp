using Integration.Contracts.Commands;
using Knjizenje.Application.Commands.StornirajStavku;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Knjizenje.API.Consumers
{
	public class StornirajStavkuConsumer : IConsumer<IStornirajStavku>
	{
		private readonly ILogger<StornirajStavkuConsumer> logger;
		private readonly IMediator mediator;

		public StornirajStavkuConsumer(IMediator mediator, ILogger<StornirajStavkuConsumer> logger)
		{
			this.mediator = mediator;
			this.logger = logger;
		}

		public async Task Consume(ConsumeContext<IStornirajStavku> context)
		{
			logger.LogTrace($"Consuming command {context.Message.CommandId}");
			var msg = context.Message;
			await mediator.Send(new StornirajStavkuCommand(msg.CommandId, msg.Version, msg.UserId, msg.IdNaloga, msg.IdStavke));
		}
	}
}
