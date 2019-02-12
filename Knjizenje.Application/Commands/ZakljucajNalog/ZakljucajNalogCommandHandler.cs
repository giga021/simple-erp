using Knjizenje.Domain.Entities.FinNalogAggregate;
using Knjizenje.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Knjizenje.Application.Commands.ZakljucajNalog
{
	public class ZakljucajNalogCommandHandler : IRequestHandler<ZakljucajNalogCommand, bool>
	{
		private readonly ILogger<ZakljucajNalogCommandHandler> logger;
		private readonly IFinNalogRepository nalogRepo;

		public ZakljucajNalogCommandHandler(IFinNalogRepository nalogRepo,
			ILogger<ZakljucajNalogCommandHandler> logger)
		{
			this.nalogRepo = nalogRepo;
			this.logger = logger;
		}

		public async Task<bool> Handle(ZakljucajNalogCommand cmd, CancellationToken cancellationToken)
		{
			var nalog = await nalogRepo.GetAsync(new FinNalogId(cmd.IdNaloga));

			if (nalog == null)
				throw new NalogNePostojiException(cmd.IdNaloga);

			nalog.Zakljucaj();
			await nalogRepo.SaveAsync(nalog, cmd.CommandId, cmd.Version, cmd.UserId);
			return true;
		}
	}
}
