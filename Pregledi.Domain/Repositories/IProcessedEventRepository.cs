using Pregledi.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Pregledi.Domain.Repositories
{
	public interface IProcessedEventRepository
	{
		void Add(ProcessedEvent evnt);
		ProcessedEvent GetLastProcessed(string originalStream);
		Task<bool> IsProcessedAsync(string stream, Guid eventId);
	}
}
