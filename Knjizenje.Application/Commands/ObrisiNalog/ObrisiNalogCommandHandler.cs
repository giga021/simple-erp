using Knjizenje.Domain.Entities.FinNalogAggregate;
using Knjizenje.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Knjizenje.Application.Commands.ObrisiNalog
{
	public class ObrisiNalogCommandHandler : IRequestHandler<ObrisiNalogCommand, bool>
	{
		private readonly ILogger<ObrisiNalogCommandHandler> logger;
		private readonly IFinNalogRepository nalogRepo;

		public ObrisiNalogCommandHandler(IFinNalogRepository nalogRepo,
			ILogger<ObrisiNalogCommandHandler> logger)
		{
			this.nalogRepo = nalogRepo;
			this.logger = logger;
		}

		public async Task<bool> Handle(ObrisiNalogCommand cmd, CancellationToken cancellationToken)
		{
			var nalog = await nalogRepo.GetAsync(new FinNalogId(cmd.IdNaloga));

			if (nalog == null)
				throw new NalogNePostojiException(cmd.IdNaloga);

			nalog.Obrisi();
			await nalogRepo.SaveAsync(nalog, cmd.CommandId, cmd.Version, cmd.UserId);
			return true;
		}
	}
}
