using Integration.Contracts.Commands;
using Knjizenje.Application.Commands.OtkljucajNalog;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Knjizenje.API.Consumers
{
	public class OtkljucajNalogConsumer : IConsumer<IOtkljucajNalog>
	{
		private readonly ILogger<OtkljucajNalogConsumer> logger;
		private readonly IMediator mediator;

		public OtkljucajNalogConsumer(IMediator mediator, ILogger<OtkljucajNalogConsumer> logger)
		{
			this.mediator = mediator;
			this.logger = logger;
		}

		public async Task Consume(ConsumeContext<IOtkljucajNalog> context)
		{
			logger.LogTrace($"Consuming command {context.Message.CommandId}");
			var msg = context.Message;
			await mediator.Send(new OtkljucajNalogCommand(msg.CommandId, msg.Version, msg.UserId, msg.IdNaloga));
		}
	}
}
