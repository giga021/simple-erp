using Integration.Contracts.Commands;
using Knjizenje.Application.Commands.IzmeniZaglavljeNaloga;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Knjizenje.API.Consumers
{
	public class IzmeniZaglavljeNalogaConsumer : IConsumer<IIzmeniZaglavljeNaloga>
	{
		private readonly ILogger<IzmeniZaglavljeNalogaConsumer> logger;
		private readonly IMediator mediator;

		public IzmeniZaglavljeNalogaConsumer(IMediator mediator, ILogger<IzmeniZaglavljeNalogaConsumer> logger)
		{
			this.mediator = mediator;
			this.logger = logger;
		}

		public async Task Consume(ConsumeContext<IIzmeniZaglavljeNaloga> context)
		{
			logger.LogTrace($"Consuming command {context.Message.CommandId}");
			var msg = context.Message;
			await mediator.Send(new IzmeniZaglavljeNalogaCommand(msg.CommandId, msg.Version, msg.UserId,
				msg.IdNaloga, msg.IdTip, msg.DatumNaloga, msg.Opis));
		}
	}
}
