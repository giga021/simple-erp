using System;

namespace Knjizenje.Persistence.EventSourcing
{
	public class EventMetadata
	{
		public Guid CommandId { get; }
		public string UserId { get; }

		public EventMetadata(Guid commandId, string userId)
		{
			this.CommandId = commandId;
			this.UserId = userId;
		}
	}
}
