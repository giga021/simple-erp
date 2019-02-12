using Knjizenje.Domain.Entities.FinNalogAggregate;
using Knjizenje.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Knjizenje.Application.Commands.ProknjiziStavku
{
	public class ProknjiziStavkuCommandHandler : IRequestHandler<ProknjiziStavkuCommand, bool>
	{
		private readonly ILogger<ProknjiziStavkuCommandHandler> logger;
		private readonly IFinNalogRepository nalogRepo;

		public ProknjiziStavkuCommandHandler(IFinNalogRepository nalogRepo,
			ILogger<ProknjiziStavkuCommandHandler> logger)
		{
			this.nalogRepo = nalogRepo;
			this.logger = logger;
		}

		public async Task<bool> Handle(ProknjiziStavkuCommand cmd, CancellationToken cancellationToken)
		{
			var nalog = await nalogRepo.GetAsync(new FinNalogId(cmd.IdNaloga));

			if (nalog == null)
				throw new NalogNePostojiException(cmd.IdNaloga);

			var stavka = new FinStavka(cmd.IdKonto, cmd.Duguje, cmd.Potrazuje, cmd.Opis);
			nalog.ProknjiziStavku(stavka);
			await nalogRepo.SaveAsync(nalog, cmd.CommandId, cmd.Version, cmd.UserId);
			return true;
		}
	}
}
