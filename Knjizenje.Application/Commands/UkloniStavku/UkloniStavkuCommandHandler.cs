using Knjizenje.Domain.Entities.FinNalogAggregate;
using Knjizenje.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Knjizenje.Application.Commands.UkloniStavku
{
	public class UkloniStavkuCommandHandler : IRequestHandler<UkloniStavkuCommand, bool>
	{
		private readonly ILogger<UkloniStavkuCommandHandler> logger;
		private readonly IFinNalogRepository nalogRepo;

		public UkloniStavkuCommandHandler(IFinNalogRepository nalogRepo,
			ILogger<UkloniStavkuCommandHandler> logger)
		{
			this.nalogRepo = nalogRepo;
			this.logger = logger;
		}

		public async Task<bool> Handle(UkloniStavkuCommand cmd, CancellationToken cancellationToken)
		{
			var nalog = await nalogRepo.GetAsync(new FinNalogId(cmd.IdNaloga));

			if (nalog == null)
				throw new NalogNePostojiException(cmd.IdNaloga);

			var stavka = nalog.Stavke.SingleOrDefault(x => x.Id == cmd.IdStavke);
			nalog.UkloniStavku(stavka);
			await nalogRepo.SaveAsync(nalog, cmd.CommandId, cmd.Version, cmd.UserId);
			return true;
		}
	}
}
