using Knjizenje.Domain.DTO;
using Knjizenje.Domain.Entities.FinNalogAggregate;
using Knjizenje.Persistence.EventSourcing;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Knjizenje.Persistence.Repositories
{
	public class FinNalogRepository : EventSourceRepository<FinNalog, FinNalogId>, IFinNalogRepository
	{
		private readonly IEventStore eventStore;

		public FinNalogRepository(IEventStore eventStore) : base(eventStore)
		{
			this.eventStore = eventStore;
		}

		public async Task<FinNalogId> GetPostojeciAsync(TipNaloga tip, DateTime datumNaloga)
		{
			if (tip == null)
				throw new ArgumentNullException(nameof(tip));

			var zaglavlja = await eventStore.GetProjectionAsync<ZaglavljeNaloga>(Projections.ZaglavljaNaloga);
			var nalog = zaglavlja.Where(x => x.DatumNaloga == datumNaloga && x.IdTip == tip.Id).SingleOrDefault();
			return nalog?.IdNaloga;
		}

		public async Task<ZaglavljeNaloga> GetZaglavljeAsync(FinNalogId id)
		{
			if (id == null)
				throw new ArgumentNullException(nameof(id));

			var zaglavlja = await eventStore.GetProjectionAsync<ZaglavljeNaloga>(Projections.ZaglavljaNaloga);
			var nalog = zaglavlja.Where(x => x.IdNaloga == id).SingleOrDefault();
			return nalog;
		}

		public async Task<ZaglavljeNaloga> GetZaglavljeAsync(TipNaloga tip, DateTime datumNaloga)
		{
			if (tip == null)
				throw new ArgumentNullException(nameof(tip));

			var zaglavlja = await eventStore.GetProjectionAsync<ZaglavljeNaloga>(Projections.ZaglavljaNaloga);
			var nalog = zaglavlja.Where(x => x.DatumNaloga == datumNaloga && x.IdTip == tip.Id).SingleOrDefault();
			return nalog;
		}
	}
}
