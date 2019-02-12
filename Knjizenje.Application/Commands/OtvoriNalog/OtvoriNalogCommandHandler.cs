using Knjizenje.Domain.Entities.FinNalogAggregate;
using Knjizenje.Domain.Exceptions;
using Knjizenje.Domain.Seedwork;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Knjizenje.Application.Commands.OtvoriNalog
{
	public class OtvoriNalogCommandHandler : IRequestHandler<OtvoriNalogCommand, bool>
	{
		private readonly ILogger<OtvoriNalogCommandHandler> logger;
		private readonly IFinNalogRepository nalogRepo;

		public OtvoriNalogCommandHandler(IFinNalogRepository nalogRepo, ILogger<OtvoriNalogCommandHandler> logger)
		{
			this.nalogRepo = nalogRepo;
			this.logger = logger;
		}

		public async Task<bool> Handle(OtvoriNalogCommand cmd, CancellationToken cancellationToken)
		{
			var tip = TipNaloga.Get(cmd.IdTip);
			var idPostojeceg = await nalogRepo.GetPostojeciAsync(tip, cmd.DatumNaloga);
			if (idPostojeceg != null)
				throw new KnjizenjeException("Nalog sa istim zaglavljem već postoji");

			var stavke = cmd.Stavke.Select(x => new FinStavka(x.IdKonto, x.Duguje, x.Potrazuje, x.Opis));
			var nalog = new FinNalog(tip, cmd.DatumNaloga, cmd.Opis, stavke);
			await nalogRepo.SaveAsync(nalog, cmd.CommandId, cmd.Version, cmd.UserId);

			return true;
		}
	}
}
