using Pregledi.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Pregledi.Domain.Repositories
{
	public interface IStavkaFormRepository
	{
		void Add(StavkaForm stavka);
		Task<StavkaForm> GetAsync(Guid id);
		void Remove(StavkaForm stavka);
	}
}
