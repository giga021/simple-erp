using Knjizenje.Domain.Entities.FinNalogAggregate;
using Knjizenje.Domain.Exceptions;
using System;
using System.Threading.Tasks;

namespace Knjizenje.Domain.Services
{
	public interface IFinNalogService
	{
		Task IzmeniZaglavljeAsync(FinNalog nalog, TipNaloga tip, DateTime datumNaloga, string opis);
	}

	public class FinNalogService : IFinNalogService
	{
		private readonly IFinNalogRepository nalogRepo;

		public FinNalogService(IFinNalogRepository nalogRepo)
		{
			this.nalogRepo = nalogRepo;
		}

		public async Task IzmeniZaglavljeAsync(FinNalog nalog, TipNaloga tip, DateTime datumNaloga, string opis)
		{
			if (nalog == null)
				throw new ArgumentNullException(nameof(nalog));
			if (tip == null)
				throw new ArgumentNullException(nameof(tip));

			var idPostojeceg = await nalogRepo.GetPostojeciAsync(tip, datumNaloga);
			if (idPostojeceg != null && idPostojeceg != nalog.Id)
				throw new KnjizenjeException("Nalog sa istim zaglavljem već postoji");

			nalog.IzmeniZaglavlje(tip, datumNaloga, opis);
		}
	}
}
