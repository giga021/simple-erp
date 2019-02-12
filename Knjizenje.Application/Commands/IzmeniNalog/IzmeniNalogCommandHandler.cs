using Knjizenje.Domain.Entities.FinNalogAggregate;
using Knjizenje.Domain.Exceptions;
using Knjizenje.Domain.Seedwork;
using Knjizenje.Domain.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Knjizenje.Application.Commands.IzmeniNalog
{
	public class IzmeniNalogCommandHandler : IRequestHandler<IzmeniNalogCommand, bool>
	{
		private readonly ILogger<IzmeniNalogCommandHandler> logger;
		private readonly IFinNalogRepository nalogRepo;
		private readonly IFinNalogService nalogSvc;

		public IzmeniNalogCommandHandler(IFinNalogRepository nalogRepo, IFinNalogService nalogSvc,
			ILogger<IzmeniNalogCommandHandler> logger)
		{
			this.nalogRepo = nalogRepo;
			this.nalogSvc = nalogSvc;
			this.logger = logger;
		}

		public async Task<bool> Handle(IzmeniNalogCommand cmd, CancellationToken cancellationToken)
		{
			var nalog = await nalogRepo.GetAsync(new FinNalogId(cmd.IdNaloga));

			if (nalog == null)
				throw new NalogNePostojiException(cmd.IdNaloga);

			var tip = TipNaloga.Get(cmd.IdTip);
			await nalogSvc.IzmeniZaglavljeAsync(nalog, tip, cmd.DatumNaloga, cmd.Opis);
			var zaBrisanje = nalog.Stavke.Where(x => !cmd.Stavke.Select(y => y.IdStavke).Contains(x.Id)).ToList();
			foreach (var item in cmd.Stavke)
			{
				if (!nalog.Stavke.Any(x => x.Id == item.IdStavke))
				{
					nalog.ProknjiziStavku(new FinStavka(item.IdKonto, item.Duguje, item.Potrazuje, item.Opis));
				}
				if (item.Stornirana)
				{
					var s = nalog.Stavke.SingleOrDefault(x => x.Id == item.IdStavke);
					if (s != null)
						nalog.StornirajStavku(s);
				}
			}
			foreach (var item in zaBrisanje)
			{
				nalog.UkloniStavku(item);
			}
			await nalogRepo.SaveAsync(nalog, cmd.CommandId, cmd.Version, cmd.UserId);
			return true;
		}
	}
}
