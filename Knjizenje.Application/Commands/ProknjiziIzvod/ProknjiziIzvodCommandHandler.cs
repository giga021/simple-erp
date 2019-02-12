using Knjizenje.Domain.DTO;
using Knjizenje.Domain.Entities.FinNalogAggregate;
using Knjizenje.Domain.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Knjizenje.Application.Commands.ProknjiziIzvod
{
	public class ProknjiziIzvodCommandHandler : IRequestHandler<ProknjiziIzvodCommand, bool>
	{
		private readonly ILogger<ProknjiziIzvodCommandHandler> logger;
		private readonly IFinNalogRepository nalogRepo;
		private readonly IIzvodService izvodSvc;

		public ProknjiziIzvodCommandHandler(IFinNalogRepository nalogRepo, IIzvodService izvodSvc,
			ILogger<ProknjiziIzvodCommandHandler> logger)
		{
			this.nalogRepo = nalogRepo;
			this.izvodSvc = izvodSvc;
			this.logger = logger;
		}

		public async Task<bool> Handle(ProknjiziIzvodCommand cmd, CancellationToken cancellationToken)
		{
			var idNaloga = await nalogRepo.GetPostojeciAsync(TipNaloga.Izvodi, cmd.Datum);
			var stavkeIzvoda = cmd.Stavke.Select(x => new StavkaIzvoda(x.SifraPlacanja, x.Duguje, x.Potrazuje));
			var stavke = await izvodSvc.FormirajStavkeNalogaAsync(stavkeIzvoda);

			if (idNaloga != null)
			{
				var nalog = await nalogRepo.GetAsync(idNaloga);

				foreach (var stavka in stavke)
				{
					nalog.ProknjiziStavku(stavka);
				}
				await nalogRepo.SaveAsync(nalog, cmd.CommandId, nalog.Version, cmd.UserId);
			}
			else
			{
				var nalog = new FinNalog(TipNaloga.Izvodi, cmd.Datum, null, stavke);
				await nalogRepo.SaveAsync(nalog, cmd.CommandId, cmd.Version, cmd.UserId);
			}

			return true;
		}
	}
}
