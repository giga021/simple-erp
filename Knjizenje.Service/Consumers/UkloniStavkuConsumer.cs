using Integration.Contracts.Commands;
using Knjizenje.Application.Commands.UkloniStavku;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Knjizenje.API.Consumers
{
	public class UkloniStavkuConsumer : IConsumer<IUkloniStavku>
	{
		private readonly ILogger<UkloniStavkuConsumer> logger;
		private readonly IMediator mediator;

		public UkloniStavkuConsumer(IMediator mediator, ILogger<UkloniStavkuConsumer> logger)
		{
			this.mediator = mediator;
			this.logger = logger;
		}

		public async Task Consume(ConsumeContext<IUkloniStavku> context)
		{
			logger.LogTrace($"Consuming command {context.Message.CommandId}");
			var msg = context.Message;
			await mediator.Send(new UkloniStavkuCommand(msg.CommandId, msg.Version, msg.UserId, msg.IdNaloga, msg.IdStavke));
		}
	}
}
