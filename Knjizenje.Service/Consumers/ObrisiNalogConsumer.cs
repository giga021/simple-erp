using Integration.Contracts.Commands;
using Knjizenje.Application.Commands.ObrisiNalog;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Knjizenje.API.Consumers
{
	public class ObrisiNalogConsumer : IConsumer<IObrisiNalog>
	{
		private readonly ILogger<ObrisiNalogConsumer> logger;
		private readonly IMediator mediator;

		public ObrisiNalogConsumer(IMediator mediator, ILogger<ObrisiNalogConsumer> logger)
		{
			this.mediator = mediator;
			this.logger = logger;
		}

		public async Task Consume(ConsumeContext<IObrisiNalog> context)
		{
			logger.LogTrace($"Consuming command {context.Message.CommandId}");
			var msg = context.Message;
			await mediator.Send(new ObrisiNalogCommand(msg.CommandId, msg.Version, msg.UserId, msg.IdNaloga));
		}
	}
}
