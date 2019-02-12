using System;
using System.Threading.Tasks;

namespace Knjizenje.Domain.Seedwork
{
	public interface IAggregateRepository<TAggregate, TKey>
		where TAggregate : Entity<TKey>
		where TKey : IAggregateId, IEquatable<TKey>
	{
		Task<TAggregate> GetAsync(TKey id);
		Task SaveAsync(AggregateRoot<TAggregate, TKey> aggregate, Guid commandId, long? version, string userId);
	}
}
