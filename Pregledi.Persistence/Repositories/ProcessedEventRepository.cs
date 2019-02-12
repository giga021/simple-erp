using Microsoft.EntityFrameworkCore;
using Pregledi.Domain.Entities;
using Pregledi.Domain.Repositories;
using Pregledi.Persistence.Context;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pregledi.Persistence.Repositories
{
	public class ProcessedEventRepository : IProcessedEventRepository
	{
		private readonly PreglediContext context;

		public ProcessedEventRepository(PreglediContext context)
		{
			this.context = context;
		}

		public void Add(ProcessedEvent evnt)
		{
			context.ProcessedEvents.Add(evnt);
		}

		public ProcessedEvent GetLastProcessed(string originalStream)
		{
			return context.ProcessedEvents
				.Where(x => x.OriginalStream == originalStream)
				.OrderByDescending(x => x.Checkpoint)
				.FirstOrDefault();
		}

		public async Task<bool> IsProcessedAsync(string stream, Guid eventId)
		{
			return await context.ProcessedEvents.AnyAsync(x => x.Stream == stream && x.Id == eventId);
		}
	}
}
