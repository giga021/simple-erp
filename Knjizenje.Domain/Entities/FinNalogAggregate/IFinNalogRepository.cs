using Knjizenje.Domain.DTO;
using Knjizenje.Domain.Seedwork;
using System;
using System.Threading.Tasks;

namespace Knjizenje.Domain.Entities.FinNalogAggregate
{
	public interface IFinNalogRepository : IAggregateRepository<FinNalog, FinNalogId>
	{
		Task<FinNalogId> GetPostojeciAsync(TipNaloga tip, DateTime datumNaloga);
		Task<ZaglavljeNaloga> GetZaglavljeAsync(FinNalogId id);
		Task<ZaglavljeNaloga> GetZaglavljeAsync(TipNaloga tip, DateTime datumNaloga);
	}
}
