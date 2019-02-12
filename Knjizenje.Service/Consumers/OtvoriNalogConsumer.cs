using Integration.Contracts.Commands;
using Knjizenje.Application.Commands.DTO;
using Knjizenje.Application.Commands.OtvoriNalog;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Knjizenje.API.Consumers
{
	public class OtvoriNalogConsumer : IConsumer<IOtvoriNalog>
	{
		private readonly ILogger<OtvoriNalogConsumer> logger;
		private readonly IMediator mediator;

		public OtvoriNalogConsumer(IMediator mediator, ILogger<OtvoriNalogConsumer> logger)
		{
			this.mediator = mediator;
			this.logger = logger;
		}

		public async Task Consume(ConsumeContext<IOtvoriNalog> context)
		{
			logger.LogTrace($"Consuming command {context.Message.CommandId}");
			var msg = context.Message;
			var stavke = msg.Stavke.Select(s =>
				new StavkaDTO(s.IdStavke, s.IdKonto, s.Duguje, s.Potrazuje, s.Opis, s.Stornirana)).ToList();
			await mediator.Send(new OtvoriNalogCommand(msg.CommandId, msg.UserId, msg.IdTip,
				msg.DatumNaloga, msg.Opis, stavke));
		}
	}
}
