using Knjizenje.Domain.Entities.FinNalogAggregate;
using Knjizenje.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Knjizenje.Application.Commands.OtkljucajNalog
{
	public class OtkljucajNalogCommandHandler : IRequestHandler<OtkljucajNalogCommand, bool>
	{
		private readonly ILogger<OtkljucajNalogCommandHandler> logger;
		private readonly IFinNalogRepository nalogRepo;

		public OtkljucajNalogCommandHandler(IFinNalogRepository nalogRepo,
			ILogger<OtkljucajNalogCommandHandler> logger)
		{
			this.nalogRepo = nalogRepo;
			this.logger = logger;
		}

		public async Task<bool> Handle(OtkljucajNalogCommand cmd, CancellationToken cancellationToken)
		{
			var nalog = await nalogRepo.GetAsync(new FinNalogId(cmd.IdNaloga));

			if (nalog == null)
				throw new NalogNePostojiException(cmd.IdNaloga);

			nalog.Otkljucaj();
			await nalogRepo.SaveAsync(nalog, cmd.CommandId, cmd.Version, cmd.UserId);
			return true;
		}
	}
}
