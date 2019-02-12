using Integration.Contracts.Commands;
using Knjizenje.Application.Commands.ProknjiziIzvod;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Knjizenje.API.Consumers
{
	public class ProknjiziIzvodConsumer : IConsumer<IProknjiziIzvod>
	{
		private readonly ILogger<ProknjiziIzvodConsumer> logger;
		private readonly IMediator mediator;

		public ProknjiziIzvodConsumer(IMediator mediator, ILogger<ProknjiziIzvodConsumer> logger)
		{
			this.mediator = mediator;
			this.logger = logger;
		}

		public async Task Consume(ConsumeContext<IProknjiziIzvod> context)
		{
			logger.LogTrace($"Consuming command {context.Message.CommandId}");
			var msg = context.Message;
			var stavke = msg.Stavke.Select(s =>
				new StavkaIzvodaDTO(s.SifraPlacanja, s.Duguje, s.Potrazuje)).ToList();
			await mediator.Send(new ProknjiziIzvodCommand(msg.CommandId, msg.Version, msg.UserId, msg.Datum, stavke));
		}
	}
}
