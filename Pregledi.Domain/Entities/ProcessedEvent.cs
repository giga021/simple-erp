using Pregledi.Domain.Seedwork;
using System;

namespace Pregledi.Domain.Entities
{
	public class ProcessedEvent : Entity<Guid>
	{
		public string OriginalStream { get; set; }
		public string Stream { get; set; }
		public long? Checkpoint { get; set; }
		public long? CommitPosition { get; set; }
		public long? PreparePosition { get; set; }
		public DateTime Created { get; set; }

		public ProcessedEvent() { }

		public ProcessedEvent(Guid id, string originalStream, string stream,
			long? checkpoint, long? commitPosition, long? preparePosition, DateTime created)
		{
			this.Id = id;
			this.OriginalStream = originalStream;
			this.Stream = stream;
			this.Checkpoint = checkpoint;
			this.CommitPosition = commitPosition;
			this.PreparePosition = preparePosition;
			this.Created = created;
		}
	}
}
