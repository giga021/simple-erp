using System.Collections.Generic;

namespace Knjizenje.Domain.Seedwork
{

	public interface IAggregateRoot
	{
		IReadOnlyCollection<EventBase> UncommittedEvents { get; }
		void ApplyEvent<TEvent>(TEvent evnt)
			where TEvent : EventBase;
		void ClearUncommittedEvents();
	}
}
