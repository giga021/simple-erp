using Integration.Contracts.Commands;
using Knjizenje.Application.Commands.ProknjiziStavku;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Knjizenje.API.Consumers
{
	public class ProknjiziStavkuConsumer : IConsumer<IProknjiziStavku>
	{
		private readonly ILogger<ProknjiziStavkuConsumer> logger;
		private readonly IMediator mediator;

		public ProknjiziStavkuConsumer(IMediator mediator, ILogger<ProknjiziStavkuConsumer> logger)
		{
			this.mediator = mediator;
			this.logger = logger;
		}

		public async Task Consume(ConsumeContext<IProknjiziStavku> context)
		{
			logger.LogTrace($"Consuming command {context.Message.CommandId}");
			var msg = context.Message;
			await mediator.Send(new ProknjiziStavkuCommand(msg.CommandId, msg.Version, msg.UserId, msg.IdNaloga,
				msg.IdKonto, msg.Duguje, msg.Potrazuje, msg.Opis));
		}
	}
}
