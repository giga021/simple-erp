using Knjizenje.Domain.Entities.FinNalogAggregate;
using Knjizenje.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Knjizenje.Application.Commands.StornirajStavku
{
	public class StornirajStavkuCommandHandler : IRequestHandler<StornirajStavkuCommand, bool>
	{
		private readonly ILogger<StornirajStavkuCommandHandler> logger;
		private readonly IFinNalogRepository nalogRepo;

		public StornirajStavkuCommandHandler(IFinNalogRepository nalogRepo,
			ILogger<StornirajStavkuCommandHandler> logger)
		{
			this.nalogRepo = nalogRepo;
			this.logger = logger;
		}

		public async Task<bool> Handle(StornirajStavkuCommand cmd, CancellationToken cancellationToken)
		{
			var nalog = await nalogRepo.GetAsync(new FinNalogId(cmd.IdNaloga));

			if (nalog == null)
				throw new NalogNePostojiException(cmd.IdNaloga);

			var stavka = nalog.Stavke.SingleOrDefault(x => x.Id == cmd.IdStavke);
			nalog.StornirajStavku(stavka);
			await nalogRepo.SaveAsync(nalog, cmd.CommandId, cmd.Version, cmd.UserId);
			return true;
		}
	}
}
