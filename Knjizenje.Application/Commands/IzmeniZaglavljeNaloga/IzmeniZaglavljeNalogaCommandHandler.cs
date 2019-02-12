using Knjizenje.Domain.Entities.FinNalogAggregate;
using Knjizenje.Domain.Exceptions;
using Knjizenje.Domain.Seedwork;
using Knjizenje.Domain.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Knjizenje.Application.Commands.IzmeniZaglavljeNaloga
{
	public class IzmeniZaglavljeNalogaCommandHandler : IRequestHandler<IzmeniZaglavljeNalogaCommand, bool>
	{
		private readonly ILogger<IzmeniZaglavljeNalogaCommandHandler> logger;
		private readonly IFinNalogRepository nalogRepo;
		private readonly IFinNalogService nalogSvc;

		public IzmeniZaglavljeNalogaCommandHandler(IFinNalogRepository nalogRepo, IFinNalogService nalogSvc,
			ILogger<IzmeniZaglavljeNalogaCommandHandler> logger)
		{
			this.nalogRepo = nalogRepo;
			this.nalogSvc = nalogSvc;
			this.logger = logger;
		}

		public async Task<bool> Handle(IzmeniZaglavljeNalogaCommand cmd, CancellationToken cancellationToken)
		{
			var nalog = await nalogRepo.GetAsync(new FinNalogId(cmd.IdNaloga));

			if (nalog == null)
				throw new NalogNePostojiException(cmd.IdNaloga);

			var tip = TipNaloga.Get(cmd.IdTip);
			await nalogSvc.IzmeniZaglavljeAsync(nalog, tip, cmd.DatumNaloga, cmd.Opis);
			await nalogRepo.SaveAsync(nalog, cmd.CommandId, cmd.Version, cmd.UserId);
			return true;
		}
	}
}
