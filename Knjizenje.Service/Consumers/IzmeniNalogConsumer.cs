using Integration.Contracts.Commands;
using Knjizenje.Application.Commands.DTO;
using Knjizenje.Application.Commands.IzmeniNalog;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Knjizenje.API.Consumers
{
	public class IzmeniNalogConsumer : IConsumer<IIzmeniNalog>
	{
		private readonly ILogger<IzmeniNalogConsumer> logger;
		private readonly IMediator mediator;

		public IzmeniNalogConsumer(IMediator mediator, ILogger<IzmeniNalogConsumer> logger)
		{
			this.mediator = mediator;
			this.logger = logger;
		}

		public async Task Consume(ConsumeContext<IIzmeniNalog> context)
		{
			logger.LogTrace($"Consuming command {context.Message.CommandId}");
			var msg = context.Message;
			var stavke = msg.Stavke.Select(s =>
				new StavkaDTO(s.IdStavke, s.IdKonto, s.Duguje, s.Potrazuje, s.Opis, s.Stornirana)).ToList();
			await mediator.Send(new IzmeniNalogCommand(msg.CommandId, msg.Version, msg.UserId,
				msg.IdNaloga, msg.IdTip, msg.DatumNaloga, msg.Opis, stavke));
		}
	}
}
