using Integration.Contracts.Commands;
using Knjizenje.Application.Commands.ZakljucajNalog;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Knjizenje.API.Consumers
{
	public class ZakljucajNalogConsumer : IConsumer<IZakljucajNalog>
	{
		private readonly ILogger<ZakljucajNalogConsumer> logger;
		private readonly IMediator mediator;

		public ZakljucajNalogConsumer(IMediator mediator, ILogger<ZakljucajNalogConsumer> logger)
		{
			this.mediator = mediator;
			this.logger = logger;
		}

		public async Task Consume(ConsumeContext<IZakljucajNalog> context)
		{
			logger.LogTrace($"Consuming command {context.Message.CommandId}");
			var msg = context.Message;
			await mediator.Send(new ZakljucajNalogCommand(msg.CommandId, msg.Version, msg.UserId, msg.IdNaloga));
		}
	}
}
