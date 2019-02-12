using Knjizenje.Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knjizenje.Persistence.EventSourcing
{
	public class EventSourceRepository<TAggregate, TKey> : IAggregateRepository<TAggregate, TKey>
		where TAggregate : Entity<TKey>
		where TKey : IAggregateId, IEquatable<TKey>
	{
		private readonly IEventStore eventStore;

		public EventSourceRepository(IEventStore eventStore)
		{
			this.eventStore = eventStore;
		}

		public async Task<TAggregate> GetAsync(TKey id)
		{
			if (id == null)
				throw new ArgumentNullException(nameof(id));

			var events = await eventStore.ReadEventsAsync(id.IdAsString());
			var aggregate = Reconstruct(events);
			return aggregate;
		}

		public async Task SaveAsync(AggregateRoot<TAggregate, TKey> aggregate, Guid commandId, long? version, string userId)
		{
			if (aggregate == null)
				throw new ArgumentNullException(nameof(aggregate));

			await eventStore.WriteEventsAsync(aggregate.Id.IdAsString(), aggregate.UncommittedEvents, version, commandId, userId);
			aggregate.ClearUncommittedEvents();
		}

		protected TAggregate Reconstruct(IList<EventBase> events)
		{
			if (events == null)
				throw new ArgumentNullException(nameof(events));

			IAggregateRoot aggregate = AggregateRoot<TAggregate, TKey>.CreateEmpty();
			foreach (var item in events)
			{
				aggregate.ApplyEvent(item);
			}

			return (TAggregate)aggregate;
		}
	}
}
